using System.Linq;

namespace SendColorBot.ColorSpaces
{
    public class Rgb : ColorSpace
    {
        public Rgb() : base("RGB", 3, new []{255, 255, 255})
        {
            ImageSharpRatio = new[] {255F, 255, 255};
        }

        public override bool Verify(float[] colors)
        {
            if (colors.Any(color => color % 1 != 0))
                return false;
            
            return base.Verify(colors);
        }
    }
}