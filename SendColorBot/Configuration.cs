using System;
using Microsoft.Extensions.Configuration;

namespace SendColorBot
{
    class Configuration
    {
        public static IConfigurationRoot Root { get; private set; }

        public static void SetUp()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json");
            
            Root = builder.Build();
        }
    }
}
