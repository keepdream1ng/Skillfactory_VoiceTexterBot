using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBotApp.Configuration;

namespace VoiceTexterBotApp.Controllers
{
    public class DefaultMessageController : BaseController , IDefaultMessageController
    {
        protected override string _returnMessage { get; set; } = "Unsupported format message received.";
        public DefaultMessageController(AppSettings appSettings, ITelegramBotClient telegramBotClient) : base(appSettings, telegramBotClient) { }
    }
}
