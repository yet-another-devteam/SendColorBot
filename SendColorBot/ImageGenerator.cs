using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SendColorBot
{
    public class ImageGenerator
    {
        private string fileserverPath;
        private string fileserverIp;
        
        public ImageGenerator(string fileserverPath, string fileserverIp)
        {
            this.fileserverPath = fileserverPath;
            this.fileserverIp = fileserverIp;
        }
        /// <summary>
        /// Generates image with color and returns link
        /// </summary>
        /// <returns>Link on image</returns>
        public string Generate(Rgba32 color)
        {
            string filename = $@"{color.ToHex()}.png";
            
            var image = new Image<Rgba32> (500, 500);
            image.Mutate(ctx => ctx.Fill(color));
            image.Save($"{fileserverPath}/{filename}", 
                new PngEncoder());

            return $@"{fileserverIp}/{filename}";
        }
    }
}