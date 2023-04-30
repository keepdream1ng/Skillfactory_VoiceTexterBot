using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public interface IVoiceMessageController
    {
        Task HandleAsync(Message message, CancellationToken ct);
    }
}
