using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendColorBot.ColorSpaces;
using Telegram.Bot.Args;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot.Services
{
    class UpdateHandler
    {
        private readonly List<ColorSpace> colorSpaces;
        private readonly InlineCardProcessor cardProcessor;
        
        public UpdateHandler()
        {
            cardProcessor = new InlineCardProcessor();
            
            // List of available color spaces
            colorSpaces = new List<ColorSpace>()
            {
                new Cmyk()
            };
        }
        public async Task OnMessage(MessageEventArgs m)
        {
            
        }

        public async Task OnInlineQuery(InlineQueryEventArgs q)
        {
            // Stores requested string from user
            string request = q.InlineQuery.Query;

            List<InlineQueryResultBase> result = new List<InlineQueryResultBase>();

            // An array that stores colors from the request
            int[] colors = GetColors(request);
            
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

            throw new Exception("Unknown color");
        }
    }
}    