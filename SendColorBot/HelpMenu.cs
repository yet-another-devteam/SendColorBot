using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace SendColorBot
{
    public class HelpMenu
    {
        readonly TelegramBotClient _client;
        readonly string _demoVideo;
        readonly string _helpText;
        readonly InlineKeyboardMarkup _markup;

        public HelpMenu(TelegramBotClient client, string demoVideoVideo, string helpText)
        {
            _helpText = helpText;
            _client = client;
            _demoVideo = demoVideoVideo;

            _markup = new InlineKeyboardMarkup(new InlineKeyboardButton
            {
                Text = "Try it now!",
                SwitchInlineQuery = "#30A3E6"
            });
        }

        public async Task HandleHelpRequest(long chatId)
        {
            await _client.SendVideoAsync(chatId, _demoVideo, caption: _helpText, replyMarkup: _markup);
        }
    }
}