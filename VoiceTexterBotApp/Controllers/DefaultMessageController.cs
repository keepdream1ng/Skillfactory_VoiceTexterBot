using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp.Controllers
{
    public class DefaultMessageController : BaseController , IDefaultMessageController
    {
        protected override string _returnMessage { get; set; } = "Unsupported format message received.";
        public DefaultMessageController(AppSettings appSettings, ITelegramBotClient telegramBotClient, ISimpleLogger logger, IStorage memoryStorage)
            : base(appSettings, logger, telegramBotClient, memoryStorage) { }
    }
}
