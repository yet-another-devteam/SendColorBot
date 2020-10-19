using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;
namespace SendColorBot
{
    public class ColorSpacesManager
    {
        // Key: initial color space name
        // Value: array of methods, where each item is a target color space converter
        private readonly Dictionary<string, Dictionary<string, MethodInfo>> _colorConverters;
        
        readonly Dictionary<string, ConstructorInfo> _colorConstructors;
        readonly ColorSpaceConverter _colorSpaceConverter = new ColorSpaceConverter();
        readonly Dictionary<string, Type> _colorSpacesAndTypes;
 
        public ColorSpacesManager(ICollection<string> colorSpacesNames)
        {
            Log.Information("Getting color spaces");
            List<Type> colorTypes = typeof(Color).Assembly.GetTypes()
                .Where(type => (type.Namespace == "SixLabors.ImageSharp.ColorSpaces")
                            && colorSpacesNames.Contains(type.Name.ToUpperInvariant())).ToList();

            _colorSpacesAndTypes = colorTypes.ToDictionary(x => x.Name.ToUpperInvariant(), x => x);

            _colorConstructors = _colorSpacesAndTypes
                .Select(type => (type.Key, type.Value.GetConstructors()
                    .FirstOrDefault(ctor => ctor.GetParameters()
                        .All(parameter => parameter.ParameterType == typeof(float)))))
                .Where(kvp => kvp.Item2 != null)
                .ToDictionary(kvp => kvp.Item1, kvp => kvp.Item2);

            Log.Information("Getting color space converters from ImageSharp...");
            _colorConverters = _colorSpacesAndTypes
	            .ToDictionary(type => type.Key,
		            type =>
			            typeof(ColorSpaceConverter)
				            .GetMethods().Where(method => {
					            var parameters = method.GetParameters();
					            return method.Name.StartsWith("To", StringComparison.InvariantCulture)
                                       && parameters.Length == 1
                                       && parameters[0].ParameterType == type.Value.MakeByRefType()
                                       && colorSpacesNames.Contains(method.ReturnType.Name.ToUpperInvariant());
				            }).ToDictionary(x => x.ReturnType.Name.ToUpperInvariant(), x => x));
        }

        static Rgba32 ConvertRgbToRgba32(Rgb rgbSpace)
        {
            return new Rgba32(rgbSpace.R, rgbSpace.G, rgbSpace.B);
        }

        public Rgba32 CreateRgba32(string colorSpace, float[] colors)
        {
            if (colorSpace == "RGB")
                return new Rgba32(colors[0], colors[1], colors[2]);
            
            if (!_colorConstructors.TryGetValue(colorSpace, out ConstructorInfo constructorInfo))
                throw new ArgumentException("Invalid color space", nameof(colorSpace));

            if (!_colorConverters.TryGetValue(colorSpace, out var colorConverters))
                throw new ArgumentException("Invalid color space", nameof(colorSpace));

            if (constructorInfo.GetParameters().Length != colors.Length)
                throw new ArgumentException("Invalid length of colors argument", nameof(colors));

            if (!colorConverters.TryGetValue("RGB", out var colorConverter)) 
	            throw new ArgumentException("Not found suitable converter");

            // ReSharper disable once PossibleNullReferenceException
            var rgbSpace = (Rgb) colorConverter
                .Invoke(_colorSpaceConverter, new[] {constructorInfo.Invoke(colors.Cast<object>().ToArray())});
    
            return ConvertRgbToRgba32(rgbSpace);
        }
        
        public float[] CalculateInDifferentColorSpace(float[] colors, string initColorSpace, string finalColorSpace)
        {
	        if (initColorSpace == finalColorSpace)
		        return colors;

            if (finalColorSpace == "RGB")
            {
                var rgba = CreateRgba32(initColorSpace, colors);
                return new[] {rgba.R / 255F, rgba.G / 255F, rgba.B / 255F};
            }
            
            if (!_colorConstructors.TryGetValue(initColorSpace, out ConstructorInfo initConstructorInfo) 
                ||!_colorConverters.TryGetValue(initColorSpace, out var colorConverters))
            {
                throw new ArgumentException("Invalid initial color space", nameof(initColorSpace));
            } 

            if (initConstructorInfo.GetParameters().Length != colors.Length)
                throw new ArgumentException("Invalid length of colors argument", nameof(colors));

            // ReSharper disable once AssignNullToNotNullAttribute
            if(!colorConverters.TryGetValue(finalColorSpace, out var colorConverter))
                throw new ArgumentException("Invalid final color");

            MethodInfo finalColorSpaceToString = _colorSpacesAndTypes[finalColorSpace].GetMethod(nameof(ToString), BindingFlags.Instance | BindingFlags.Public);
            
            if(finalColorSpaceToString == null)
                throw new ArgumentException("Final color space does not have ToString method", nameof(colors));

            // ReSharper disable once PossibleNullReferenceException SuggestVarOrType_SimpleTypes
            var finalColorObject = colorConverter.Invoke(_colorSpaceConverter, new[] {initConstructorInfo.Invoke(colors.Cast<object>().ToArray())});
            string finalColorString = (string)finalColorSpaceToString.Invoke(finalColorObject, null);

            return ColorUtils.GetColorsFromToString(finalColorString);
        }
    }   
}