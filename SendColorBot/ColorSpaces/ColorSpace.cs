using System.Linq;
using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot.ColorSpaces
{
    public abstract class ColorSpace
    {
        
        public string Name { get; } // User friendly name of color space
        private readonly int colorsCount;    // How many primary colors color space use
        private readonly int[] maxColors;    // Max value for each color
        public int[] MinColors;    // Min value for each color
        

        protected ColorSpace(string name, int colorsCount, int[] maxColors)
        {
            Name = name;
            this.colorsCount  = colorsCount;
            this.maxColors = maxColors;
            MinColors = new[] {0, 0, 0, 0}; // By default min value for each color is 0 
        }
        
        /// <summary>
        /// Verifies that the color space can have the entered colors
        /// </summary>
        public bool Verify(int[] colors)
        {
            if (colors.Length != colorsCount)
                return false;

            // Verifies that all colors match the minimum and maximum possible
            return !colors.Where((t, i) => !(t >= MinColors[i] && t <= maxColors[i])).Any();
        }

        public abstract Rgba32 ConvertToRgb32(int[] colors);
    }
}