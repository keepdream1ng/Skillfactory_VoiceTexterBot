using Telegram.Bot;
using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public class InlineKeyboardController : BaseController, IInlineKeyboardController
    {
        protected override string _returnMessage { get; set; } = "Button press detected";
        public InlineKeyboardController(ITelegramBotClient telegramBotClient) : base(telegramBotClient) { }

        public async Task HandleAsync(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");

            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, _returnMessage, cancellationToken: ct);
        }
    }
}
