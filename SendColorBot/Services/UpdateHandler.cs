using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot.Services
{
    class UpdateHandler {
        private readonly InlineCardProcessor _cardProcessor;
        private readonly List<string> _colorSpaces;
        private readonly ColorSpacesManager _colorSpacesManager;
        private readonly HelpMenu _helpMenu;
        
        public UpdateHandler()
        {
            _cardProcessor = new InlineCardProcessor();
            _colorSpaces = Configuration.Root.GetSection("ColorSpaces").Get<List<string>>().Select(colorSpace => colorSpace.ToUpperInvariant()).ToList();
            _colorSpacesManager = new ColorSpacesManager(_colorSpaces);
            _helpMenu = new HelpMenu(Bot.Client, Configuration.Root["HelpMenu:DemoVideo"], Configuration.Texts["en-us:HelpMenu"]);
        }

        public async Task OnMessage(Message message)
        {
            if (message.Chat.Type == ChatType.Private)
                await _helpMenu.HandleHelpRequest(message.Chat.Id);
        }
        
        public async Task OnInlineQuery(InlineQuery q)
        {
            // Stores the string requested by the user
            string request = q.Query;
            
            if (string.IsNullOrEmpty(request))
                return;

            // An array that stores colors from the request
            float[] colors;

            try
            {
                colors = GetColors(request.TrimStart('#'));
            }
            catch
            {
                Log.Warning($"Can't parse colors from string: [{request}]");
                return;
            }

            // Inline card list 
            List<InlineQueryResultBase> result = new List<InlineQueryResultBase>();

            byte index = 0;
            foreach (var colorSpace in _colorSpaces) {
                Rgba32 color;
                try {
                    color = _colorSpacesManager.CreateColorAndConvertToRgba32(colorSpace, colors);
                } catch (ArgumentException) {
                    continue;
                }
                
                result.AddRange(_cardProcessor.ProcessInlineCardsForColorSpace(index++, color, colorSpace, colors));
            }

            try {
                await Bot.Client.AnswerInlineQueryAsync(q.Id, result);
            } catch {
                // ignored
            }

            Log.Information("Send answer with " + result.Count + " results to " + q.From.Id);
        }
        
        private float[] GetColors(string requestString)
        {
            if (Rgba32.TryParseHex(requestString, out var rgba))
            {
                return new[] { rgba.R / 255.0f, rgba.G / 255.0f, rgba.B / 255.0f };
            }
            
            var colorRegex = new Regex(@"-*(\d+)", RegexOptions.Compiled);
            // Selects all colors and creates an array of them

            var colors = colorRegex.Matches(requestString)
                .Select(m => int.Parse(m.Value, NumberStyles.Integer, CultureInfo.InvariantCulture) / 100.0f)
                .ToArray();

            return colors;
        }
    }
}    