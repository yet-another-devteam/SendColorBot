using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SendColorBot
{
    public class ColorUtils
    {
        public static float[] GetColorsFromString(string requestString)
        {
            //var colorRegex = new Regex(@"([\d(.,)]+)", RegexOptions.Compiled);
            var colorRegex = new Regex(@"-*(\d+)", RegexOptions.Compiled);
            // Selects all colors and creates an array of them

            var colors = colorRegex
                .Matches(requestString)
                .Select(m => float.Parse(m.Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                .ToArray();

            return colors;
        }

        public static float[] GetColorsFromToString(string toStringResult)
        {
            return toStringResult[(toStringResult.IndexOf('(') + 1)..^1]
                .Split(", ")
                .Select(x => float.Parse(x, CultureInfo.InvariantCulture))
                .ToArray();
        }

    }
}