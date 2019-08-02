using System.Numerics;
using SendColorBot.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot.ColorSpaces
{
    public class Cmyk : ColorSpace
    {
        public Cmyk() : base("CMYK", 4, new []{100, 100, 100, 100})
        {
            
        }

        protected override Rgba32 ConvertToRgb32(int[] colors)
        {
            return new Rgba32(new Vector4(
                colors[0], colors[1], colors[2], colors[3]));
        }
    }
}