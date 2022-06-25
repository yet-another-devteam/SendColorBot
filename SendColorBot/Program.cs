using System;
using System.Threading;
using System.Threading.Tasks;
using SendColorBot;
using SendColorBot.Services;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

Console.WriteLine("Starting up...");
// Parse configuration.json
Configuration.SetUp();
// Creates new instance of bot client with token
Bot.Authorize(Configuration.Root["Telegram:Token"]);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(Configuration.Root)
    .CreateLogger();

var updates = new UpdateHandler();

Log.Information("Initialization complete");

Log.Information("Starting to receive messages...");

Bot.Client.StartReceiving(cancellationToken: CancellationToken.None,
    updateHandler: UpdateHandler,
    errorHandler: (_, _, _) => Task.CompletedTask, 
    receiverOptions: new ()
    {
        AllowedUpdates = new[] {UpdateType.Message, UpdateType.CallbackQuery},
        Offset = -1
    });


Log.Information("Receiving messages...");

await Task.Delay(-1);

async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
{
    switch (update.Type)
    {
        case UpdateType.Message:
            await updates.OnMessage(update.Message);
            break;
        case UpdateType.InlineQuery:
            await updates.OnInlineQuery(update.InlineQuery);
            break;
        case UpdateType.ChosenInlineResult:
            await updates.OnChosenResult(update.ChosenInlineResult);
            break;
        case UpdateType.CallbackQuery:
            await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "Who asked you to click it?", cancellationToken: cancellationToken);
            break;
        
    }

}