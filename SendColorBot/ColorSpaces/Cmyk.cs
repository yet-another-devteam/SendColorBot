using System.Numerics;
using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot.ColorSpaces
{
    public class Cmyk : ColorSpace
    {
        public Cmyk() : base("CMYK", 4, new []{100, 100, 100, 100})
        {
            
        }

        public override Rgba32 ConvertToRgb32(int[] colors)
        {
            byte r = (byte)(255 * (1 - colors[0]) * (1 - colors[3]));
            byte g = (byte)(255 * (1 - colors[1]) * (1 - colors[3]));
            byte b = (byte)(255 * (1 - colors[2]) * (1 - colors[3]));
            
            return new Rgba32(r, g, b);
        }
    }
}