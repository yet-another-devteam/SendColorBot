using System.Linq;

namespace SendColorBot.ColorSpaces
{
    public class Hsl : ColorSpace
    {
        public Hsl() : base("HSL", 3, new[] {360, 100, 100})
        {
            ImageSharpRatio = new[] {1F, 100, 100};
        }

        public override float[] ConvertToImageSharpFormat(float[] colors)
        {
            float[] result = colors.ToArray();
            for (int i = 0; i < colorsCount; i++)
            {
                // This check is used when user input for S and L is not in %
                // As ImageSharp use the same format, we should skip conversion
                /*if(result[i] < 1 && i > 1)
                    continue;*/
                
                result[i] /= ImageSharpRatio[i];
            }

            return result;
        }
    }
}