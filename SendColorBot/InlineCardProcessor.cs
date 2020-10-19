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

        public InlineCardProcessor(CaptionGenerator captionGenerator)
        {
            _captionGenerator = captionGenerator;
            _imageGeneratorClient = new ImageGeneratorClient(Configuration.Root["ImageGenerator:Domain"]);
        }

        /// <param name="cardId">Card ID</param>
        /// <param name="colors"></param>
        /// <param name="colorInRgb">With this color ImageGenerator will generate picture</param>
        /// <param name="colorSpace">Color space that used for this color</param>
        /// <returns>Card with preview image, which should be replaced with FinalMessage after sending</returns>
        public (InlineQueryResultPhoto, FinalMessage) ProcessInlineCardForColorSpace(string cardId, float[] colors, Rgba32 colorInRgb, ColorSpace colorSpace)
        {
            string previewUrl = _imageGeneratorClient.GetLink(colorInRgb, 250, 150, colorSpace.Name);
            string finalUrl = _imageGeneratorClient.GetLink(colorInRgb, 250, 150, null);
            string caption = _captionGenerator.GenerateCaption(colorSpace, colors);
            
            var card = new InlineQueryResultPhoto(cardId, previewUrl, previewUrl)
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