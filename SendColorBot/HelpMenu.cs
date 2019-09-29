using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace SendColorBot
{
    public class HelpMenu
    {
        private readonly TelegramBotClient _client;
        private readonly string _demoUrl;
        private readonly string _helpText;
        private readonly InlineKeyboardMarkup _markup;
        
        public HelpMenu(TelegramBotClient client, string demoGifUrl, string helpText)
        {
            _helpText = helpText;
            _client = client;
            _demoUrl = demoGifUrl;

            _markup = new InlineKeyboardMarkup(new InlineKeyboardButton {Text = "Try it!", SwitchInlineQuery = "#30A3E6"});
        }
        
        public async Task HandleHelpRequest(long chatId)
        {
            await _client.SendVideoAsync(chatId, _demoUrl, caption:_helpText, replyMarkup: _markup);
        }
    }
}