using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot.Services
{
    class UpdateHandler
    {
        public async Task OnMessage(MessageEventArgs m)
        {

        }

        public async Task OnInlineQuery(InlineQueryEventArgs q)
        {
            
        }

        private IPixel GetColor(string requestString)
        {
            if (requestString.ToCharArray()[0] == '#')
            {
                byte[] rgb = FromHexToRgb("requestString");
                return new Rgb24(rgb[0], rgb[1], rgb[2]);
            }

            return null;
        }

        private byte[] FromHexToRgb(string hex)
        {
            IEnumerable<string> hexParts = new List<string>();
            hexParts = hex.SplitInParts(2);

            List<byte> result = new List<byte>();
            foreach (var hexPart in hexParts)
            {
                result.Add(byte.Parse(hexPart, System.Globalization.NumberStyles.HexNumber));
            }

            return result.ToArray();
        }
    }
}
