﻿using Telegram.Bot;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Services;
using VoiceTexterBotApp.Utilities;

namespace VoiceTexterBotApp.Services
{
    public class AudioFileHandler : IFileHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;

        public AudioFileHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _telegramBotClient = telegramBotClient;
        }

        public async Task Download(string fileId, CancellationToken ct)
        {
            // Генерируем полный путь файла из конфигурации
            string inputAudioFilePath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");

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

        public string Process(string languageCode)
        {
            string inputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");
            string outputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.OutputAudioFormat}");

            AudioConverter.TryConvert(inputAudioPath, outputAudioPath);

            var speechText = SpeechDetector.DetectSpeech(outputAudioPath, _appSettings.InputAudioBitrate, languageCode);
            return speechText;
        }
    }
}
