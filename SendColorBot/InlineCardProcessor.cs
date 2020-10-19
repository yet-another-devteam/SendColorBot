using SendColorBot.ColorSpaces;
using SendColorBot.Models;
using SixLabors.ImageSharp.PixelFormats;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace SendColorBot
{
    public class InlineCardProcessor
    {
        readonly IImageGeneratorClient _imageGeneratorClient;
        private readonly CaptionGenerator _captionGenerator;

        public InlineCardProcessor()
        {
            _imageGeneratorClient = new ImageGeneratorClient(Configuration.Root["ImageGenerator:Domain"]);
        }
        
        /// <param name="id">Card ID</param>
        /// <param name="color">With this color ImageGenerator will generate picture</param>
        /// <param name="colorSpace">Color space that used for this color</param>
        /// <param name="caption">Caption for sent image</param>
        /// <returns>Card with preview image, which should be replaced with FinalMessage after sending</returns>
        public (InlineQueryResultPhoto, FinalMessage) ProcessInlineCardForColorSpace(string id, Rgba32 color, ColorSpace colorSpace, string caption)
        {
            string previewUrl = _imageGeneratorClient.GetLink(color, 250, 150, colorSpace.Name);
            string finalUrl = _imageGeneratorClient.GetLink(color, 250, 150, null);

            var card = new InlineQueryResultPhoto(id, previewUrl, previewUrl)
            {
                Caption = caption,
                PhotoWidth = 250,   
                PhotoHeight = 150,
                ReplyMarkup = new InlineKeyboardMarkup(new InlineKeyboardButton {Text = "Loading...", CallbackData = "do-not-click-it"})
            };
            
            var finalMessage = new FinalMessage(finalUrl, caption);
            return (card, finalMessage);
        }
    }
}