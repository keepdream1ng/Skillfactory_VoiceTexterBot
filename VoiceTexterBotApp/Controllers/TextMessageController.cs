using Telegram.Bot;
using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public class TextMessageController : BaseController
    {
        public TextMessageController(ITelegramBotClient telegramBotClient) : base(telegramBotClient) { }
    }
}
