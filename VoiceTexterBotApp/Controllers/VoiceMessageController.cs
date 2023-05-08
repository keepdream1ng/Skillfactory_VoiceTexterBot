using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp.Controllers
{
    public class VoiceMessageController : BaseController , IVoiceMessageController
    {
        private readonly IFileHandler _audioFileHandler;
        public VoiceMessageController(AppSettings appSettings, ISimpleLogger logger, ITelegramBotClient telegramBotClient,
            IFileHandler audioFileHandler, IStorage memoryStorage) : base(appSettings, logger, telegramBotClient, memoryStorage)
        {
            _audioFileHandler = audioFileHandler;
        }
        public override async Task HandleAsync(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
            {
                _logger.Log($"Controller {GetType().Name} cant find an audio file.");
                return;
            }

            // new instance of IFilehandler, since dependensy injection gives the same one every time.
            IFileHandler AsyncFileHandler = new AudioFileHandler(_telegramClient, _appSettings);

            _logger.Log($"Controller {GetType().Name} get an voice message.");
            await AsyncFileHandler.Download(fileId, ct);
            _logger.Log($"Controller {GetType().Name} downloaded the file.");
            string textResult = await AsyncFileHandler.Process(_memoryStorage.GetSession(message.Chat.Id).LanguageCode);
            _logger.Log($"Controller {GetType().Name} conversation result is:{Environment.NewLine}{textResult}.");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, textResult, cancellationToken: ct);
        }
    }
}
