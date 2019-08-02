using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot
{
    public class InlineCardProcessor
    {
        private ImageGenerator imageGenerator;
        
        public InlineCardProcessor()
        {
            imageGenerator = new ImageGenerator(Configuration.Root["tokens:fileserver-path"], Configuration.Root["tokens:fileserver-ip"]);    
        }
        
        public InlineQueryResultPhoto ProccessInlineCard(byte id, Rgba32 color, string colorSpaceName)
        {
            string photoUrl = imageGenerator.Generate(color);
            return new InlineQueryResultPhoto(id.ToString(), photoUrl, photoUrl);
        }
    }
}   