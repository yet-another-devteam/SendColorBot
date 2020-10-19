using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot
{
    public interface IImageGeneratorClient
    {
        string GetLink(Rgba32 color, int width, int height, string name);
    }
}