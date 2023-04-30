using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public interface IDefaultMessageController
    {
        Task HandleAsync(Message message, CancellationToken ct);
    }
}