using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public interface ITextMessageController
    {
        Task HandleAsync(Message message, CancellationToken ct);
    }
}
