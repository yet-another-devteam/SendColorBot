using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot
{
    public class InlineCardProcessor
    {
        private readonly IImageGenerator _imageGenerator;
                    
        public InlineCardProcessor()
        {
            _imageGenerator = new ImageGenerator(Configuration.Root["imagegenerator:domain"]);    
        }
        
        /// <param name="id">Card ID</param>
        /// <param name="color">With this color ImageGenerator will generate picture</param>
        /// <param name="colorSpaceName">Color space that used for this color</param>
        public InlineQueryResultBase[] ProcessInlineCardsForColorSpace(int id, Rgba32 color, string colorSpaceName)
        {
            List<InlineQueryResultBase> result = new List<InlineQueryResultBase>
            {
                //ProcessTitleInlineCard(id + 1, colorSpaceName),
                ProcessPhotoInlineCard(id, color, colorSpaceName)
            };

            return result.ToArray();
        }

        private InlineQueryResultPhoto ProcessPhotoInlineCard(int id, Rgba32 color, string colorSpaceName)
        {
            var photoUrl = _imageGenerator.GetLink(color);
            return new InlineQueryResultPhoto(id.ToString(), photoUrl, photoUrl)
            {
                Caption = GenerateCaption(color, colorSpaceName),
            };
        }

        private InlineQueryResultArticle ProcessTitleInlineCard(int id, string title)
        {
            return new InlineQueryResultArticle(id.ToString(), $"{title}:", new InputTextMessageContent("You should choose color space that you want to use!"));
        }

        private string GenerateCaption(Rgba32 color, string colorSpaceName)
        {
            string hex = color.ToHex();
            // Removes alpha from HEX string if it is 255
            if (hex[hex.Length - 1] == 'F' || hex[hex.Length - 2] == 'F')
                hex = hex.Remove(hex.Length - 2);

            string rgb = $"{color.Rgb.R}, {color.Rgb.G}, {color.Rgb.B}";

            string caption = "";
            switch (colorSpaceName)
            {
                case "CMYK":
                    caption = $"CMYK: {RgbToCmyk(color)}\n";
                    break;
            }
            
            caption = caption + $"HEX: #{hex}\n" +
                                $"RGB: {rgb}\n";

            return caption;
        }

        private string RgbToCmyk(Rgba32 color)
        {
            int r = color.R, g = color.G, b = color.B;
            
            double k = Math.Min(1.0 - r / 255.0, Math.Min(1.0 - g / 255.0, 1.0 - b / 255.0));
            double c = (1.0 - r / 255.0 - k) / (1.0 - k);
            double m = (1.0 - g / 255.0 - k) / (1.0 - k);
            double y = (1.0 - b / 255.0 - k) / (1.0 - k);
            
            return $"{Math.Round(c, 2) * 100}, {Math.Round(m, 2) * 100}, {Math.Round(y, 2) * 100}, {Math.Round(k, 2) * 100}";
        }
    }
}   