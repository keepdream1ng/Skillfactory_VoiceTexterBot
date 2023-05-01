using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp.Controllers
{
    public abstract class BaseController
    {
        private readonly AppSettings _appSettings;

        protected readonly ITelegramBotClient _telegramClient;

        protected readonly ISimpleLogger _logger;
        protected virtual string _returnMessage { get; set; } = "Text message received";

        public BaseController(AppSettings appSettings, ISimpleLogger logger, ITelegramBotClient telegramBotClient)
        {
            _appSettings = appSettings;
            _telegramClient = telegramBotClient;
            _logger = logger;
        }

        public virtual async Task HandleAsync(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Controller {GetType().Name} get a message.");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, _returnMessage, cancellationToken: ct);
        }
    }
}
