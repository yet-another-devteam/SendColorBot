using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot
{
    public class InlineCardProcessor
    {
        readonly IImageGenerator _imageGenerator;

        public InlineCardProcessor()
        {
            _imageGenerator = new ImageGenerator(Configuration.Root["ImageGenerator:Domain"]);
        }

        /*
        private InlineQueryResultArticle ProcessTitleInlineCard(int id, string title)
        {
            return new InlineQueryResultArticle(id.ToString(), $"{title}:", new InputTextMessageContent("You should choose color space that you want to use!"));
        }
        */

        string GenerateCaption(Rgba32 color, string colorSpaceName, float[] colors)
        {
            string hex = color.ToHex();

            // Removes alpha from HEX string
            if (hex.Length > 6)
                hex = hex[..6];

            string rgb = $"{color.Rgb.R}, {color.Rgb.G}, {color.Rgb.B}";

            var caption = new StringBuilder();
            if (colorSpaceName != "RGB")
            {
                caption.Append(colorSpaceName);
                caption.Append(": ");
                caption.Append(string.Join(", ", colors.Select(x => (int) (x * 100))));
                caption.Append('\n');
            }

            caption.Append($"HEX: #{hex}\n");
            caption.Append($"RGB: {rgb}\n");

            return caption.ToString();
        }

        /// <param name="id">Card ID</param>
        /// <param name="color">With this color ImageGenerator will generate picture</param>
        /// <param name="colorSpaceName">Color space that used for this color</param>
        /// <param name="colors">Source color values to display for color space</param>
        public InlineQueryResultBase[] ProcessInlineCardsForColorSpace(int id, Rgba32 color, string colorSpaceName, float[] colors)
        {
            List<InlineQueryResultBase> result = new List<InlineQueryResultBase>
            {
                //ProcessTitleInlineCard(id + 1, colorSpaceName),
                ProcessPhotoInlineCard(id, color, colorSpaceName, colors)
            };

            return result.ToArray();
        }

        InlineQueryResultPhoto ProcessPhotoInlineCard(int id, Rgba32 color, string colorSpaceName, float[] colors)
        {
            string photoUrl = _imageGenerator.GetLink(color);
            return new InlineQueryResultPhoto(id.ToString(), photoUrl, photoUrl)
            {
                Caption = GenerateCaption(color, colorSpaceName, colors),
                PhotoWidth = 250,
                PhotoHeight = 150
            };
        }
    }
}