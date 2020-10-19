using System;
using System.Linq;

namespace SendColorBot
{
    public static class Utilities
    {
        private static readonly Random Random = new Random();

        public static string GetRandomHexNumber(int digits)
        {
            lock (Random)
            {
                var buffer = new byte[digits / 2];
                Random.NextBytes(buffer);
                string result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
                if (digits % 2 == 0)
                    return result;

                return result + Random.Next(16).ToString("X");
            }
        }
    }
}