namespace SendColorBot.ColorSpaces
{
    public class Hsl : ColorSpace
    {
        public Hsl() : base("HSL", 3, new []{360, 100, 100})
        {
            ImageSharpRatio = new[] {1F, 100, 100};
        }
    }
}