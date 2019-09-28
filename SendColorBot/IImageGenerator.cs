using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot
{
    public interface IImageGenerator
    {
        string GetLink(Rgba32 color);
    }
}