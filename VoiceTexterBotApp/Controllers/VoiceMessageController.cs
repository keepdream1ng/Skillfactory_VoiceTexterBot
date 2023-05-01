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
        protected override string _returnMessage { get; set; } = "Voice message downloaded.";
        public VoiceMessageController(AppSettings appSettings, ITelegramBotClient telegramBotClient,
            IFileHandler audioFileHandler, IStorage memoryStorage) : base(appSettings, telegramBotClient)
        {
            _audioFileHandler = audioFileHandler;
            _memoryStorage = memoryStorage;
        }
        public async Task HandleAsync(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;

            await _audioFileHandler.Download(fileId, ct);
            var textResult = _audioFileHandler.Process(_memoryStorage.GetSession(message.Chat.Id).LanguageCode);
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, textResult, cancellationToken: ct);
        }
    }
}
