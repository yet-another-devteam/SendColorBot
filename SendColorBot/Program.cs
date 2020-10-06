using System;
using System.Threading.Tasks;
using SendColorBot.Services;
using Serilog;
using Telegram.Bot.Types.Enums;

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

            var updates = new UpdateHandler();
            Bot.Client.OnInlineQuery += async (sender, args) => { await updates.OnInlineQuery(args.InlineQuery); };
            Bot.Client.OnMessage += async (sender, args) => { await updates.OnMessage(args.Message); };

            // Starts update receiving
            Bot.Client.StartReceiving(new[] {UpdateType.Message, UpdateType.InlineQuery});
            Log.Information("Receiving messages...");

            await Task.Delay(-1);
        }
    }
}