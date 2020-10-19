using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SendColorBot.ColorSpaces;

namespace SendColorBot
{
    public class CaptionGenerator
    {
        readonly ColorSpacesManager _colorSpacesManager;
        private readonly List<ColorSpace> _colorSpaces;
        
        public CaptionGenerator(ColorSpacesManager colorSpacesManager, List<ColorSpace> colorSpaces)
        {
            _colorSpacesManager = colorSpacesManager;
            _colorSpaces = colorSpaces;
        }
        
        public string GenerateCaption(ColorSpace initColorSpace, float[] colors)
        {
            var rgb = _colorSpacesManager.CreateRgba32(initColorSpace.Name, colors);
            var caption = new StringBuilder();
            string hex = rgb.ToHex();

            // Removes alpha from HEX string
            if (hex.Length > 6)
                hex = hex[..6];

            caption.AppendLine($"HEX: #{hex}");
            foreach (var colorSpace in _colorSpaces)
            {
                if (colorSpace.Name == initColorSpace.Name)
                    caption.AppendLine($"{colorSpace.Name}: {string.Join(", ", colorSpace.ConvertFromImageSharpFormat(colors).Select(x => Math.Round(x)))}");
                else 
                    caption.AppendLine($"{colorSpace.Name}: {string.Join(", ", colorSpace.ConvertFromImageSharpFormat(_colorSpacesManager.CalculateInDifferentColorSpace(colors, initColorSpace.Name, colorSpace.Name)).Select(x => Math.Round(x)))}");
            }
            
            return caption.ToString();
        }
    }
}