using System;
using System.Threading.Tasks;
using SendColorBot.Services;

namespace SendColorBot
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Starting up...");
            Configuration.SetUp();
            Bot.Authorize(Configuration.Root["tokens:telegram"]);
            LoggingService.StartLoggingService();

            UpdateHandler updates = new UpdateHandler();
            Bot.Client.OnMessage += async (sender, args) => { await updates.OnMessage(args); };
            Bot.Client.OnInlineQuery += async (sender, args) => { await updates.OnInlineQuery(args); };

            Bot.Client.StartReceiving();
            LoggingService.LogInfo("Receiving messages...");

            await Task.Delay(-1);
        }
    }
}
