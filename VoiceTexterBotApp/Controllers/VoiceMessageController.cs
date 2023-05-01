using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp.Controllers
{
    public class VoiceMessageController : BaseController , IVoiceMessageController
    {
        private readonly AppSettings _appSettings;
        private readonly IFileHandler _audioFileHandler;
        protected override string _returnMessage { get; set; } = "Voice message downloaded.";
        public VoiceMessageController(AppSettings appSettings, ITelegramBotClient telegramBotClient,
            IFileHandler audioFileHandler) : base(telegramBotClient)
        {
            _appSettings = appSettings;
            _audioFileHandler = audioFileHandler;
        }
        public async Task HandleAsync(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;

            await _audioFileHandler.Download(fileId, ct);

            await _telegramClient.SendTextMessageAsync(message.Chat.Id, _returnMessage, cancellationToken: ct);
        }
    }
}
