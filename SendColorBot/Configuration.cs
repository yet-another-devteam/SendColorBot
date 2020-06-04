using System;
using Microsoft.Extensions.Configuration;

namespace SendColorBot
{
    class Configuration
    {
        public static IConfigurationRoot Root { get; private set; }
        public static IConfigurationRoot Texts { get; private set; }

        public static void SetUp()
        {
            Root = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            Texts = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("texts.json")
                .Build();
        }
    }
}
