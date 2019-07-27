using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot
{
    public enum ColorSpaces
    {
        RGB24,
        RGB32
    }
    
    public class InlineCardProcessor
    {
        private ImageGenerator generator;
        
        public InlineCardProcessor()
        {
            generator = new ImageGenerator(Configuration.Root["tokens:fileserver-path"], Configuration.Root["tokens:fileserver-ip"]);    
        }
        
        public InlineQueryResultPhoto ProccessInlineCard(byte id, Rgba32 color, ColorSpaces colorSpace)
        {
            string photoUrl = generator.Generate(color);
            return new InlineQueryResultPhoto(id.ToString(), photoUrl, photoUrl);
        }
    }
}   