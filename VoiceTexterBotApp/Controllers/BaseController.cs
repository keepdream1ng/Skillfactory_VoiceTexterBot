using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBotApp.Configuration;

namespace VoiceTexterBotApp.Controllers
{
    public abstract class BaseController
    {
        private readonly AppSettings _appSettings;

        protected readonly ITelegramBotClient _telegramClient;
        protected virtual string _returnMessage { get; set; } = "Text message received";

        public BaseController(AppSettings appSettings, ITelegramBotClient telegramBotClient)
        {
            _appSettings = appSettings;
            _telegramClient = telegramBotClient;
        }

        public virtual async Task HandleAsync(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, _returnMessage, cancellationToken: ct);
        }
    }
}
