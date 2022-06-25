using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendColorBot.ColorSpaces;
using Serilog;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot.Services
{
    class UpdateHandler
    {
        readonly InlineCardProcessor _cardProcessor;
        readonly List<ColorSpace> _colorSpaces;
        readonly ColorSpacesManager _colorSpacesManager;
        readonly HelpMenu _helpMenu;
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
            _cardProcessor = new InlineCardProcessor(new CaptionGenerator(_colorSpacesManager, _colorSpaces));
            _helpMenu = new HelpMenu(Bot.Client, Configuration.Root["HelpMenu:DemoVideo"], Configuration.Texts["en-us:HelpMenu"]);
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
            var result = new List<InlineQueryResult>();

            foreach (ColorSpace colorSpace in _colorSpaces.Where(x => x.Verify(colors)))
            {
                if (fromHex && colorSpace.Name != "RGB")
                    continue;
                
                float[] formattedColors = colorSpace.ConvertToImageSharpFormat(colors);
                Rgba32 colorInRgb;
                try
                {
                    colorInRgb = _colorSpacesManager.CreateRgba32(colorSpace.Name, formattedColors);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                string resultId = Utilities.GetRandomHexNumber(8);

                var (card, finalMessage) = _cardProcessor.ProcessInlineCardForColorSpace(resultId, formattedColors, colorInRgb, colorSpace);
                
                result.Add(card);
                _resultsStorage[resultId] = finalMessage;
            }

            try
            {
                await Bot.Client.AnswerInlineQueryAsync(q.Id, result, 0, true);
            }
            catch(Exception e)
            {
                Log.Error("Got exception on answering inline query", e);
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