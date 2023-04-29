using Telegram.Bot;
using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public class DefaultMessageController : BaseController
    {
        protected override string _returnMessage { get; set; } = "Unsupported format message received.";
        public DefaultMessageController(ITelegramBotClient telegramBotClient) : base(telegramBotClient) { }
    }
}
