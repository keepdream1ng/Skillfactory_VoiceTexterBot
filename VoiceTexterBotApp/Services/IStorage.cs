using VoiceTexterBotApp.Models;

namespace VoiceTexterBotApp.Services
{
    public interface IStorage
    {
        Session GetSession(long chatId);
    }
}