using System.Linq;

namespace SendColorBot.ColorSpaces
{
    public abstract class ColorSpace
    {
        public string Name { get; } // User friendly name of color space
        private readonly int colorsCount;    // How many primary colors color space use
        private readonly int[] maxColors;    // Max value for each color
        private int[] MinColors;    // Min value for each color
        protected float[] ImageSharpRatio; 

        protected ColorSpace(string name, int colorsCount, int[] maxColors)
        {
            Name = name;
            this.colorsCount  = colorsCount;
            this.maxColors = maxColors;
            MinColors = new[] {0, 0, 0, 0}; // By default min value for each color is 0 
            ImageSharpRatio = new[] {1F, 1, 1, 1}; // By default min ratio for each color is 1. So that formats is identical
        }

        /// <summary>
        /// Verifies that the color space can have the entered colors
        /// </summary>
        public bool Verify(float[] colors)
        {
            if (colors.Length != colorsCount)
                return false;

            // Verifies that all colors match the minimum and maximum possible
            return !colors.Where((t, i) => !(t >= MinColors[i] && t <= maxColors[i])).Any();
        }

        public float[] ConvertFromImageSharpFormat(float[] colors)
        {
            float[] result = colors.ToArray();
            for (int i = 0; i < colorsCount; i++)
            {
                result[i] *= ImageSharpRatio[i];
            }

            return result;
        }
        
        public float[] ConvertToImageSharpFormat(float[] colors)
        {
            float[] result = colors.ToArray();
            for (int i = 0; i < colorsCount; i++)
            {
                result[i] /= ImageSharpRatio[i];
            }

            return result;
        }
    }
}