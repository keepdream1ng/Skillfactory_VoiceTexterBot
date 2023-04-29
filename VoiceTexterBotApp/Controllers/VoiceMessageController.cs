using Telegram.Bot;
using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public class VoiceMessageController : BaseController
    {
        protected override string _returnMessage { get; set; } = "Voice message received.";
        public VoiceMessageController(ITelegramBotClient telegramBotClient) : base(telegramBotClient) { }
    }
}
