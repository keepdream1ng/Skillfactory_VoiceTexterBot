using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTexterBotApp.Configuration;
using VoiceTexterBotApp.Controllers;
using VoiceTexterBotApp.Services;

namespace VoiceTexterBotApp
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Service is started.");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Service is stopped.");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(appSettings);
            services.AddTransient<IDefaultMessageController, DefaultMessageController>();
            services.AddTransient<IVoiceMessageController, VoiceMessageController>();
            services.AddTransient<ITextMessageController, TextMessageController>();
            services.AddTransient<IInlineKeyboardController, InlineKeyboardController>();
            services.AddTransient<IFileHandler, AudioFileHandler>();
            services.AddSingleton<IStorage, MemoryStorage>();


            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
        static AppSettings BuildAppSettings()
        {
            Console.WriteLine("Input token for bot.");
            return new AppSettings()
            {
                BotToken = Console.ReadLine(),
                AudioFileName = "audio",
                InputAudioFormat = "ogg",
                OutputAudioFormat = "wav",
                DownloadsFolder = "C:\\Users\\Public\\Downloads",
                InputAudioBitrate = 768,
            };
        }
    }
}