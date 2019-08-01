using SendColorBot.Interfaces;

namespace SendColorBot.ColorSpaces
{
    public class Cmyk : IColorSpace
    {
        public bool Verify(int[] colors)
        {
            if (colors.Length != 4)
                return false;
            
            return colors[0] >= 0 && colors[0] <= 100 && 
               colors[1] >= 0 && colors[1] <= 100 && 
               colors[2] >= 0 && colors[2] <= 100 &&
               colors[2] >= 0 && colors[3] <= 100;
        }
    }
}