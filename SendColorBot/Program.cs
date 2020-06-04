using System;
using System.Threading.Tasks;
using SendColorBot.Services;
using Serilog;

namespace SendColorBot
{
    class Program
    {
        // Entry point
        static async Task Main()
        {
            Console.WriteLine("Starting up...");
            // Parse configuration.json
            Configuration.SetUp();
            // Creates new instance of bot client with token
            Bot.Authorize(Configuration.Root["Telegram:Token"]);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration.Root)
                .CreateLogger();

            UpdateHandler updates = new UpdateHandler();
            Bot.Client.OnInlineQuery += async (sender, args) => { await updates.OnInlineQuery(args.InlineQuery); };
            Bot.Client.OnMessage += async (sender, args) => { await updates.OnMessage(args.Message); };

            // Starts update receiving
            Bot.Client.StartReceiving();
            Log.Information("Receiving messages...");

            await Task.Delay(-1);
        }
    }
}
