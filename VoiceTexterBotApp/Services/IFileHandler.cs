namespace VoiceTexterBotApp.Services
{
    public interface IFileHandler
    {
        Task Download(string fileId, CancellationToken ct);
        Task<string> Process(string param);
    }
}
