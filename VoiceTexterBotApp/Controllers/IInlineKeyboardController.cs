using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public interface IInlineKeyboardController
    {
        Task HandleAsync(CallbackQuery? callbackQuery, CancellationToken ct);
    }
}