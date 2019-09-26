using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SendColorBot.ColorSpaces;
using Telegram.Bot.Args;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot.Services
{
    class UpdateHandler
    {
        private readonly InlineCardProcessor cardProcessor;

        // List of available color spaces
        public static readonly List<ColorSpace> colorSpaces = new List<ColorSpace>()
        {
            new Cmyk(),
            new Rgb()
        };
        
        public UpdateHandler()
        {
            cardProcessor = new InlineCardProcessor();
        }

        public async Task OnInlineQuery(InlineQueryEventArgs q)
        {
            // Stores the string requested by the user
            string request = q.InlineQuery.Query;
            
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
            foreach (var colorSpace in colorSpaces)
            {
                // If color space can have these colors
                if (colorSpace.Verify(colors))
                {
                    result.Add(cardProcessor.ProcessInlineCard(
                        result.Count + 1, colorSpace.ConvertToRgb32(colors), colorSpace.Name));
                }
            }
            
            await Bot.Client.AnswerInlineQueryAsync(q.InlineQuery.Id, result);
        }
        
        private int[] GetColors(string requestString)
        {
            // If input in HEX, then translate to decimal array of colors 
            if (requestString.ToCharArray()[0] == '#')
            {
                byte[] result = {
                    Rgba32.FromHex(requestString.Remove(0)).R,
                    Rgba32.FromHex(requestString.Remove(0)).G,
                    Rgba32.FromHex(requestString.Remove(0)).B
                };
                // Translates byte array to int array and returns result
                return result.Select(x => (int) x).ToArray();
            }
            
            // Selects all colors and creates an array of them
            string[] colors = Regex.Matches(requestString, @"([0-9]){1,3}")
                .Select(m => m.Value)
                .ToArray();
            
            // Coverts string array to int array and returns
            return Array.ConvertAll(colors, int.Parse);
        }
    }
}    