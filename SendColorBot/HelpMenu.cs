using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace SendColorBot
{
    public class HelpMenu
    {
        private readonly TelegramBotClient _client;
        private readonly string _demoVideo;
        private readonly string _helpText;
        private readonly InlineKeyboardMarkup _markup;

        public HelpMenu(TelegramBotClient client, string demoVideoVideo, string helpText)
        {
            _helpText = helpText;
            _client = client;
            _demoVideo = demoVideoVideo;

            _markup = new InlineKeyboardMarkup(new InlineKeyboardButton("Try it now!")
            {
                SwitchInlineQuery = "#30A3E6"
            });
        }

        public async Task HandleHelpRequest(long chatId)
        {
            await _client.SendVideoAsync(chatId, _demoVideo, caption: _helpText, replyMarkup: _markup);
        }
    }
}