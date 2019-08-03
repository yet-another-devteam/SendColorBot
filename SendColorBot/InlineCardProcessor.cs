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
        public InlineQueryResultArticle ProcessInlineCard(int id, Rgba32 color, string colorSpaceName)
        {
            // Generates picture with color
            string photoUrl = imageGenerator.Generate(color);

            // Content for ResultArticle
            InputTextMessageContent messageContent = new InputTextMessageContent($"[ ]({photoUrl})")
            {
                ParseMode = ParseMode.Markdown, DisableWebPagePreview = false,
            };

            return new InlineQueryResultArticle(id.ToString(), colorSpaceName, messageContent)
            {
                HideUrl = true, 
                ThumbUrl = photoUrl
            };
        }
    }
}   