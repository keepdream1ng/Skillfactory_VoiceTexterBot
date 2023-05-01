using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp.Controllers
{
    public class InlineKeyboardController : BaseController, IInlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        protected override string _returnMessage { get; set; } = "Button press detected";
        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage) : base(telegramBotClient)
        {
            _memoryStorage = memoryStorage;
        }

        public async Task HandleAsync(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

            // Генерим информационное сообщение
            string languageText = callbackQuery.Data switch
            {
                "ru" => " Русский",
                "en" => " Английский",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
