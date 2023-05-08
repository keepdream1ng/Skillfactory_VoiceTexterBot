using Telegram.Bot;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Services;
using VoiceTexterBotApp.Utilities;

namespace VoiceTexterBotApp.Services
{
    public class AudioFileHandler : IFileHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;
        private string _audioFilePath { get; }

        public AudioFileHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _telegramBotClient = telegramBotClient;
            _audioFilePath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}{Guid.NewGuid().ToString()}");
        }

        public async Task Download(string fileId, CancellationToken ct)
        {
            // Генерируем полный путь файла из конфигурации
            string inputAudioFilePath = Path.Combine($"{_audioFilePath}.{_appSettings.InputAudioFormat}");

            using (FileStream destinationStream = File.Create(inputAudioFilePath))
            {
                // Загружаем информацию о файле
                var file = await _telegramBotClient.GetFileAsync(fileId, ct);
                if (file.FilePath == null)
                    return;

                // Скачиваем файл
                await _telegramBotClient.DownloadFileAsync(file.FilePath, destinationStream, ct);
            }
        }

        public async Task<string> Process(string languageCode)
        {
            string inputAudioPath = Path.Combine($"{_audioFilePath}.{_appSettings.InputAudioFormat}");
            string outputAudioPath = Path.Combine($"{_audioFilePath}.{_appSettings.OutputAudioFormat}");

            AudioConverter.TryConvert(inputAudioPath, outputAudioPath);

            var speechText = SpeechDetector.DetectSpeech(outputAudioPath, _appSettings.InputAudioBitrate, languageCode);

            // Asynchronous clean up.
            Task.Run(() => File.Delete(inputAudioPath));
            Task.Run(() => File.Delete(outputAudioPath));

            return speechText;
        }
    }
}
