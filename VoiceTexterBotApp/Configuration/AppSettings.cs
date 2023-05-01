namespace VoiceTexterBotApp.Configuration
{
    public class AppSettings
    {
        /// <summary>
        /// Telegram API bot token.
        /// </summary>
        public string BotToken { get; set; }

        /// <summary>
        /// Folder path for download audio files.
        /// </summary>
        public string DownloadsFolder { get; set; }

        /// <summary>
        /// File Name for download.
        /// </summary>
        public string AudioFileName { get; set; }

        /// <summary>
        /// Audio message file format.
        /// </summary>
        public string InputAudioFormat { get; set; }

        /// <summary>
        /// File format converter does conversion in to.
        /// </summary>
        public string OutputAudioFormat { get; set; }

        /// <summary>
        /// .wav file bit rate.
        /// </summary>
        public float InputAudioBitrate { get; set; }
    }
}