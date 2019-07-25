using System;
using System.Collections.Generic;

namespace SendColorBot
{
    public static class Extensions
    {
        // https://stackoverflow.com/a/4133475
        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength) 
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

    }
}