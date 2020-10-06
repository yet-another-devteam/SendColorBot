using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot
{
    public class ImageGenerator : IImageGenerator
    {
        readonly string _domain;

        public ImageGenerator(string imageGeneratorDomain)
        {
            _domain = imageGeneratorDomain;
        }

        public string GetLink(Rgba32 color) => $"{_domain}?width=250&height=150&color={color.ToHex()}";
    }
}