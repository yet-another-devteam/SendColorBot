namespace SendColorBot.ColorSpaces
{
    public class Rgb : ColorSpace
    {
        public Rgb() : base("RGB", 3, new []{255, 255, 255})
        {
            ImageSharpRatio = new[] {255F, 255, 255};
        }
    }
}