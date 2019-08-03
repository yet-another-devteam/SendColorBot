using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot.ColorSpaces
{
    public class Rgb : ColorSpace
    {
        public Rgb() : base("RGB", 3, new []{255, 255, 255})
        {
            
        }
        
        public override Rgba32 ConvertToRgb32(int[] colors)
        {
            return new Rgba32(colors[0], colors[1], colors[2]);
        }

    }
}