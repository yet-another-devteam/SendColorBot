using System;
using System.Threading.Tasks;
using SendColorBot.Services;

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
            Bot.Authorize(Configuration.Root["tokens:telegram"]);
            LoggingService.StartLoggingService();

            UpdateHandler updates = new UpdateHandler();
            Bot.Client.OnInlineQuery += async (sender, args) => { await updates.OnInlineQuery(args.InlineQuery); };

            // Starts update receiving
            Bot.Client.StartReceiving();
            LoggingService.LogInfo("Receiving messages...");

            await Task.Delay(-1);
        }
    }
}
