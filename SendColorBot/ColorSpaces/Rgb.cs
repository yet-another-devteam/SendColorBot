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
            byte[] bytes = { (byte)colors[0], (byte)colors[1], (byte)colors[2]};
            return new Rgba32(bytes[0], bytes[1], bytes[2]);
        }
    }
}