using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;

namespace SendColorBot
{
    public class ColorSpacesManager
    {
        readonly Dictionary<string, ConstructorInfo> _colorConstructors;
        readonly Dictionary<string, MethodInfo> _colorConverters;
        readonly ColorSpaceConverter _colorSpaceConverter = new ColorSpaceConverter();

        public ColorSpacesManager(List<string> colorSpaces)
        {
            List<Type> _colorTypes = typeof(Color).Assembly.GetTypes().Where(type => (type.Namespace == "SixLabors.ImageSharp.ColorSpaces") && colorSpaces.Contains(type.Name.ToUpperInvariant())).ToList();

            _colorConstructors = _colorTypes
                .Select(type => (type.Name.ToUpperInvariant(), type.GetConstructors()
                    .FirstOrDefault(ctor => ctor.GetParameters().All(parameter => parameter.ParameterType == typeof(float)))))
                .Where(kvp => kvp.Item2 != null).ToDictionary(kvp => kvp.Item1, kvp => kvp.Item2);

            _colorConverters = _colorTypes
                .Select(type => (type.Name.ToUpperInvariant(), typeof(ColorSpaceConverter)
                    .GetMethod("ToRgb", BindingFlags.Public | BindingFlags.Instance, null, new[] {type.MakeByRefType()}, null)))
                .Where(kvp => kvp.Item2 != null).ToDictionary(kvp => kvp.Item1, kvp => kvp.Item2);

            var method = new DynamicMethod("ReturnRgb", typeof(Rgb), new[] {typeof(Rgb)});
            ILGenerator ilGenerator = method.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ret);

            _colorConverters["RGB"] = method;
        }

        static Rgba32 ConvertRgbToRgba32(Rgb rgbSpace) => new Rgba32(rgbSpace.R, rgbSpace.G, rgbSpace.B);

        public Rgba32 CreateColorAndConvertToRgba32(string colorSpace, float[] colors)
        {
            if (!_colorConstructors.TryGetValue(colorSpace, out ConstructorInfo constructorInfo))
            {
                throw new ArgumentException("Invalid color space", nameof(colorSpace));
            }

            if (!_colorConverters.TryGetValue(colorSpace, out MethodInfo colorConverter))
            {
                throw new ArgumentException("Invalid color space", nameof(colorSpace));
            }

            if (constructorInfo.GetParameters().Length != colors.Length)
            {
                throw new ArgumentException("Invalid length of colors argument", nameof(colors));
            }

            // ReSharper disable once PossibleNullReferenceException
            var rgbSpace = (Rgb) colorConverter.Invoke(_colorSpaceConverter, new[] {constructorInfo.Invoke(colors.Cast<object>().ToArray())});

            return ConvertRgbToRgba32(rgbSpace);
        }
    }
}