using System.Linq;
using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot.ColorSpaces
{
    public abstract class ColorSpace
    {
        public string Name { get; }
        private int colorsCount;
        private int[] maxColors;
        
        protected ColorSpace(string name, int colorsCount, int[] maxColors)
        {
            this.Name = name;
            this.colorsCount = colorsCount;
            this.maxColors = maxColors;
        }
        
        public virtual bool Verify(int[] colors)
                                {
                                    if (colors.Length != colorsCount)
                                        return false;
                        
                                    // Checks is all colors fit in possible maximum
            return !colors.Where((t, i) => !(t >= 0 && t <= maxColors[i])).Any();
        }

        protected abstract Rgba32 ConvertToRgb32(int[] colors);
    }
}