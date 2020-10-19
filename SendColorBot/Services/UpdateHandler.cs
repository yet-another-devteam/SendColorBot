using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendColorBot.ColorSpaces;
using Serilog;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using SendColorBot.Models;
namespace SendColorBot.Services
{
    class UpdateHandler
    {
        readonly InlineCardProcessor _cardProcessor;
        readonly List<ColorSpace> _colorSpaces;
        readonly ColorSpacesManager _colorSpacesManager;
        readonly HelpMenu _helpMenu;
        private ImageGeneratorClient _imageGenerator;
        private ResultsStorage _resultsStorage;

        public UpdateHandler()
        {
            _colorSpaces = new List<ColorSpace>
            {
                new Rgb(),
                new Hsl()
            };

            var colorSpaceNames = _colorSpaces.Select(x => x.Name).ToList();
            
            _colorSpacesManager = new ColorSpacesManager(colorSpaceNames);
            _cardProcessor = new InlineCardProcessor();
            _helpMenu = new HelpMenu(Bot.Client, Configuration.Root["HelpMenu:DemoVideo"], Configuration.Texts["en-us:HelpMenu"]);
            _imageGenerator = new ImageGeneratorClient(Configuration.Root["ImageGenerator:Domain"]);
            _resultsStorage = new ResultsStorage();
        }

        public async Task OnInlineQuery(InlineQuery q)
        {
            // Stores the string requested by the user
            string request = q.Query;

            if (string.IsNullOrEmpty(request))
                return;

            // An array that stores colors from the request
            float[] colors;
            bool fromHex = false;
            
            try
            {
                if (Rgba32.TryParseHex(request, out Rgba32 rgba))
                {
                    colors = new[] {(float)rgba.R, rgba.G, rgba.B};
                    fromHex = true;
                }
                else
                {
                    colors = ColorUtils.GetColorsFromString(request);
                }
            }
            catch
            {
                Log.Warning($"Can't parse colors from string: [{request}]");
                return;
            }

            // Inline card list 
            List<InlineQueryResultBase> result = new List<InlineQueryResultBase>();

            foreach (ColorSpace colorSpace in _colorSpaces.Where(x => x.Verify(colors)))
            {
                if (fromHex && colorSpace.Name != "RGB")
                    continue;
                
                Rgba32 color;
                try
                {
                    color = _colorSpacesManager.CreateRgba32(colorSpace.Name, colorSpace.ConvertToImageSharpFormat(colors));
                }
                catch (ArgumentException)
                {
                    continue;
                }

                float[] formattedColors = colorSpace.ConvertToImageSharpFormat(colors);

                string id = Utilities.GetRandomHexNumber(8);
                string caption = _captionGenerator.GenerateCaption(colorSpace, formattedColors);

                InlineQueryResultPhoto card = _cardProcessor.ProcessInlineCardForColorSpace(id, color, colorSpace, caption);
                result.Add(card);
                _resultsStorage[id] = new FinalMessage(_imageGenerator.GetLink(color, 250, 150, null), caption);
            }

            try
            {
                await Bot.Client.AnswerInlineQueryAsync(q.Id, result, 0, true);
            }
            catch
            {
                // ignored
            }

            Log.Information("Send answer with " + result.Count + " results to " + q.From.Id);
        }

        public async Task OnMessage(Message message)
        {
            if (message.Chat.Type == ChatType.Private)
            {
                await _helpMenu.HandleHelpRequest(message.Chat.Id);
            }
        }

        public async Task OnChosenResult(ChosenInlineResult argsChosenInlineResult)
        {
            string resultId = argsChosenInlineResult.ResultId;
            string messageId = argsChosenInlineResult.InlineMessageId;
            Log.Information($"Chose result {resultId} with message {messageId}");

            if (!_resultsStorage.TryRemove(resultId, out var finalMessage))
                return;

            var (image, caption) = finalMessage;
            await Bot.Client.EditMessageMediaAsync(messageId, new InputMediaPhoto(new InputMedia(image)));
            await Bot.Client.EditMessageCaptionAsync(messageId, caption);
            Log.Information("Edited");
        }
    }
}