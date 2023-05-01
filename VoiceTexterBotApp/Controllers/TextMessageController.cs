using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp.Controllers
{
    public class TextMessageController : BaseController , ITextMessageController
    {
        public TextMessageController(AppSettings appSettings,ISimpleLogger logger, ITelegramBotClient telegramBotClient) : base(appSettings, logger, telegramBotClient) { }

        public override async Task HandleAsync(Message message, CancellationToken ct)
        {
            _logger.Log($"Controller {GetType().Name} get a message \"{message.Text}\".");
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Русский" , $"ru"),
                        InlineKeyboardButton.WithCallbackData($" English" , $"en")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот превращает аудио в текст.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Можно записать сообщение и переслать другу, если лень печатать.{Environment.NewLine}",
                        cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте аудио для превращения в текст.", cancellationToken: ct);
                    break;
            }
        }
    }
}
