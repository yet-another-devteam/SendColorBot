using SendColorBot.Interfaces;

namespace SendColorBot.ColorSpaces
{
    public class Rgb : IColorSpace
    {
        public bool Verify(int[] colors)
        {
            if (colors.Length != 3)
                return false;

            return colors[0] >= 0 && colors[0] <= 255 &&
                   colors[1] >= 0 && colors[1] <= 255 &&
                   colors[2] >= 0 && colors[2] <= 255;
        }
    }
}