using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp.Controllers
{
    public class VoiceMessageController : BaseController , IVoiceMessageController
    {
        private readonly IFileHandler _audioFileHandler;
        private readonly IStorage _memoryStorage;
        public VoiceMessageController(AppSettings appSettings, ISimpleLogger logger, ITelegramBotClient telegramBotClient,
            IFileHandler audioFileHandler, IStorage memoryStorage) : base(appSettings, logger, telegramBotClient)
        {
            _audioFileHandler = audioFileHandler;
            _memoryStorage = memoryStorage;
        }
        public override async Task HandleAsync(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
            {
                _logger.Log($"Controller {GetType().Name} cant find an audio file.");
                return;
            }

            _logger.Log($"Controller {GetType().Name} get an voice message.");
            await _audioFileHandler.Download(fileId, ct);
            _logger.Log($"Controller {GetType().Name} downloaded the file.");
            var textResult = _audioFileHandler.Process(_memoryStorage.GetSession(message.Chat.Id).LanguageCode);
            _logger.Log($"Controller {GetType().Name} conversation result is:{Environment.NewLine}{textResult}.");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, textResult, cancellationToken: ct);
        }
    }
}
