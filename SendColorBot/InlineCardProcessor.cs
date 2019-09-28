using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.InlineQueryResults;

namespace SendColorBot
{
    public class InlineCardProcessor
    {
        private readonly IImageGenerator imageGenerator;
                    
        public InlineCardProcessor()
        {
            imageGenerator = new ImageGenerator(Configuration.Root["imagegenerator:domain"]);    
        }
        
        /// <param name="id">Card ID</param>
        /// <param name="color">With this color ImageGenerator will generate picture</param>
        /// <param name="colorSpaceName">Color space that used for this color</param>
        public InlineQueryResultPhoto ProcessInlineCard(int id, Rgba32 color, string colorSpaceName)
        {
            // Generates picture with color
            string photoUrl = imageGenerator.GetLink(color);
                
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
    }
}   