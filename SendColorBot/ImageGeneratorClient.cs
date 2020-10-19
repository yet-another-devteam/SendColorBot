using Flurl;
using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot
{
    public class ImageGeneratorClient : IImageGeneratorClient
    {
        private readonly string _domain;

        public ImageGeneratorClient(string imageGeneratorDomain)
        {
            _domain = imageGeneratorDomain;
        }

        public string GetLink(Rgba32 color, int width, int height, string name)
        {
            return _domain.SetQueryParams(new
            {
                width,
                height,
                color = color.ToHex(),
                caption = name
            });
        }
    }
}