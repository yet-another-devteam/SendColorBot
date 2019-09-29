using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SendColorBot.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot.Services
{
    class UpdateHandler
    {
        private readonly InlineCardProcessor cardProcessor;
        private readonly HelpMenu _helpMenu;
        
        // List of available color spaces
        private static readonly List<ColorSpace> colorSpaces = new List<ColorSpace>
        {
            new Cmyk(),
            new Rgb()
        };
        
        public UpdateHandler()
        {
            cardProcessor = new InlineCardProcessor();
            _helpMenu = new HelpMenu(Bot.Client, Configuration.Root["helpmenu:demovideo"], Configuration.Root["helpmenu:text"]);
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
            int[] colors;

            try
            {
                 colors = GetColors(request);
            }
            catch
            {
                Console.WriteLine($"Can't parse colors from string: [{request}]");
                return;
            }

            // Inline card list 
            List<InlineQueryResultBase> result = new List<InlineQueryResultBase>();
            
            foreach (var colorSpace in colorSpaces.Where(colorSpace => colorSpace.Verify(colors)))
            {
                result.AddRange(cardProcessor.ProcessInlineCardsForColorSpace(
                    result.Count == 0 ? 0 : + 2, colorSpace.ConvertToRgb32(colors), colorSpace.Name));
            }
            
            await Bot.Client.AnswerInlineQueryAsync(q.Id, result);
        }
        
        private int[] GetColors(string requestString)
        {
            if (HexColorString(requestString))
            {
                return GetColorsFromHex(requestString);
            }
            
            // Selects all colors and creates an array of them
            string[] colors = Regex.Matches(requestString, @"([0-9--]){1,}")
                .Select(m => m.Value)
                .ToArray();
            
            // Coverts string array to int array and returns
            return Array.ConvertAll(colors, int.Parse);
        }

        private bool HexColorString(string hexString)
        {
            Regex regex = new Regex(@"[0-9A-Fa-f]{1,8}");
            var matches = regex.Matches(hexString);
            
            string match = matches.Count == 1 ? matches[0].Value : null;

            return match != null && hexString.Length - match.Length <= 1;
        }
        
        private int[] GetColorsFromHex(string hexString)
        {
            var rgba = Rgba32.FromHex(hexString);
                
            byte[] result = { rgba.R, rgba.G, rgba.B };
            // Translates byte array to int array and returns result
            return result.Select(x => (int) x).ToArray();

        }
    }
}    