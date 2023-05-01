using System.Collections.Concurrent;
using VoiceTexterBotApp.Models;

namespace VoiceTexterBotApp.Services
{
    public class MemoryStorage : IStorage
    {
        /// <summary>
        /// Session storage.
        /// </summary>
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        public Session GetSession(long chatId)
        {
            // Returning existing session. 
            if (_sessions.ContainsKey(chatId))
            {
                return _sessions[chatId];
            }

            // Creating new one otherwise.
            var newSession = new Session() { LanguageCode = "ru" };
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}