using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot
{
    public class InlineCardProcessor
    {
        private readonly ImageGenerator imageGenerator;
                    
        public InlineCardProcessor()
        {
            imageGenerator = new ImageGenerator(Configuration.Root["tokens:fileserver-path"], Configuration.Root["tokens:fileserver-ip"]);    
        }
        
        /// <param name="id">Card ID</param>
        /// <param name="color">With this color ImageGenerator will generate picture</param>
        /// <param name="colorSpaceName">Color space that used for this color</param>
        public InlineQueryResultPhoto ProcessInlineCard(int id, Rgba32 color, string colorSpaceName)
        {
            // Generates picture with color
            string photoUrl = imageGenerator.Generate(color);
                
            return new InlineQueryResultPhoto(id.ToString(), photoUrl, photoUrl)
            {
                Caption = GenerateCaption(color),
                Description = colorSpaceName    // Unfortunately, it doesn't work
            };
        }
        
        private string GenerateCaption(Rgba32 color)
        {
            return $"HEX: #{color.ToHex()}\n" +
                   $"RGB: {color.Rgb.R}, {color.Rgb.G}, {color.Rgb.B} \n";
        }

        #region Old code
        
        /*
        /// <param name="id">Card ID</param>
        /// <param name="color">With this color ImageGenerator will generate picture</param>
        /// <param name="colorSpaceName">Color space that used for this color</param>
        public InlineQueryResultArticle ProcessInlineCard(int id, Rgba32 color, string colorSpaceName)
        {
            // Generates picture with color
            string photoUrl = imageGenerator.Generate(color);

            // Content for ResultArticle
            InputTextMessageContent messageContent = new InputTextMessageContent(GenerateDescription(color, photoUrl))
            {
                ParseMode = ParseMode.Markdown, DisableWebPagePreview = false,
            };

            return new InlineQueryResultArticle(id.ToString(), colorSpaceName, messageContent)
            {
                HideUrl = true, 
                ThumbUrl = photoUrl
            };
        }
        */
        
        #endregion
    }
}   