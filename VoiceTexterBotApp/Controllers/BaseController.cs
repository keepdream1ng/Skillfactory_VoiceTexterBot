using Telegram.Bot;
using Telegram.Bot.Types;

namespace VoiceTexterBotApp.Controllers
{
    public abstract class BaseController
    {
        private readonly ITelegramBotClient _telegramClient;
        protected virtual string _returnMessage { get; set; } = "Получено сообщение";

        public string Hi = "hi";

        public BaseController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }

        public virtual async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, _returnMessage, cancellationToken: ct);
        }
    }
}
