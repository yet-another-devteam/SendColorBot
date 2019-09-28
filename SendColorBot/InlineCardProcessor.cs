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
                ProcessTitleInlineCard(id + 1, colorSpaceName),
                ProcessPhotoInlineCard(id, color)
            };

            return result.ToArray();
        }

        private InlineQueryResultPhoto ProcessPhotoInlineCard(int id, Rgba32 color)
        {
            var photoUrl = _imageGenerator.GetLink(color);
            return new InlineQueryResultPhoto(id.ToString(), photoUrl, photoUrl)
            {
                Caption = GenerateCaption(color),
            };
        }

        private InlineQueryResultArticle ProcessTitleInlineCard(int id, string title)
        {
            return new InlineQueryResultArticle(id.ToString(), $"{title}:", new InputTextMessageContent("You should choose color space that you want to use!"));
        }

        private string GenerateCaption(Rgba32 color)
        {
            return $"HEX: #{color.ToHex()}\n" +
                   $"RGB: {color.Rgb.R}, {color.Rgb.G}, {color.Rgb.B} \n";
        }
    }
}   