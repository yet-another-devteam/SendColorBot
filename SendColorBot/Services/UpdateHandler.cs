using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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

        private Rgba32 GetColor(string requestString)
        {
            if (requestString.ToCharArray()[0] == '#')
            {
                return Rgba32.FromHex(requestString.Remove(0));
            }

            return new Rgba32(0, 0, 0);
        }

        private IImage GenerateImage(Rgba32 color)
        {
            var image = new Image<Rgba32> (500, 500);
            image.Mutate(ctx => ctx.Fill(color));

            return image;
        }
    }
}
