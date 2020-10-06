using Telegram.Bot;

namespace SendColorBot
{
    static class Bot
    {
        public static TelegramBotClient Client { get; private set; }

        public static void Authorize(string token)
        {
            Client = new TelegramBotClient(token);
        }
    }
}