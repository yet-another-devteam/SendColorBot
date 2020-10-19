using System;
using System.Threading.Tasks;
using SendColorBot.Services;
using Serilog;
using Telegram.Bot.Types.Enums;

namespace SendColorBot
{
    static class Program
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
            Bot.Client.OnInlineQuery += async (_, args) => { await updates.OnInlineQuery(args.InlineQuery); };
            Bot.Client.OnMessage += async (_, args) => { await updates.OnMessage(args.Message); };
            Bot.Client.OnInlineResultChosen += async (_, args) => { await updates.OnChosenResult(args.ChosenInlineResult); };
            
            Bot.Client.OnCallbackQuery += async (_, args) => { await Bot.Client.AnswerCallbackQueryAsync(args.CallbackQuery.Id, "Who asked you to click it?"); };
            Log.Information("Initialization complete");
            
            Log.Information("Starting to receive messages...");
            Bot.Client.StartReceiving(new[] {UpdateType.Message, UpdateType.InlineQuery, UpdateType.ChosenInlineResult, UpdateType.CallbackQuery});
            Log.Information("Receiving messages...");

            await Task.Delay(-1);
        }
    }
}