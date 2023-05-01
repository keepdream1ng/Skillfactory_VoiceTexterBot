using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTexterBotApp.Controllers;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp
{
    internal class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramClient;
        private ISimpleLogger _logger;

        private IInlineKeyboardController _inlineKeyboardController;
        private ITextMessageController _textMessageController;
        private IVoiceMessageController _voiceMessageController;
        private IDefaultMessageController _defaultMessageController;

        public Bot(
            ITelegramBotClient telegramClient,
            ISimpleLogger logger,
            IInlineKeyboardController inlineKeyboardController,
            ITextMessageController textMessageController,
            IVoiceMessageController voiceMessageController,
            IDefaultMessageController defaultMessageController)
        {
            _telegramClient = telegramClient;
            _logger = logger;
            _inlineKeyboardController = inlineKeyboardController;
            _textMessageController = textMessageController;
            _voiceMessageController = voiceMessageController;
            _defaultMessageController = defaultMessageController;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new Telegram.Bot.Polling.ReceiverOptions() { AllowedUpdates = { } }, 
                cancellationToken: stoppingToken);

            _logger.Log("Bot started.");
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.HandleAsync(update.CallbackQuery, cancellationToken);
                return;
            }

            // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Voice:
                        await _voiceMessageController.HandleAsync(update.Message, cancellationToken);
                        return;
                    case MessageType.Text:
                        await _textMessageController.HandleAsync(update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessageController.HandleAsync(update.Message, cancellationToken);
                        return;
                }
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.Log(errorMessage);

            _logger.Log("Waiting 10 seconds to reconnect.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}