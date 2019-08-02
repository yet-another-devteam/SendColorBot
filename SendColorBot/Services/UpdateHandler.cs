using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using SendColorBot.ColorSpaces;
using SendColorBot.Interfaces;
using Telegram.Bot.Args;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot.Services
{
    class UpdateHandler
    {
        private readonly List<ColorSpace> colorSpaces;

        public UpdateHandler()
        {
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
            List<InlineQueryResultBase> result = new List<InlineQueryResultBase>();
            foreach (var colorSpace in colorSpaces)
            {
                result.Add();
            }
        }

        private void GetColorSpace()
        {
            
        }
        private Rgba32 GetColor(string requestString)
        {
            if (requestString.ToCharArray()[0] == '#')
            {
                return Rgba32.FromHex(requestString.Remove(0));
            }

            throw new Exception("Unknown color");
        }
    }
}    