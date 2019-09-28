using System;
using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot
{
    public class ImageGenerator : IImageGenerator
    {
        private string _domain;
        
        public ImageGenerator(string imageGeneratorDomain)
        {
            _domain = imageGeneratorDomain;
        }
        
        public string GetLink(Rgba32 color)
        {
            return $"{_domain}?width=250&height=250&color={Convert.ToDecimal(color.R)}{Convert.ToDecimal(color.G)}{Convert.ToDecimal(color.B)}";
        }
    }
}