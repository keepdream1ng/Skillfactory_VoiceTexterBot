using System;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
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

            ISimpleLogger logService = new Logger();

            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) => ConfigureServices(hostContext, services))
                .ConfigureServices((hostContext, services) => services.AddSingleton(logService))
                .UseConsoleLifetime()
                .Build();


            logService.Log("Service is started.");
            await host.RunAsync();
            logService.Log("Service is stopped.");
        }

        static void ConfigureServices(HostBuilderContext hostContext,IServiceCollection services)
        {

            AppSettings appSettings = BuildAppSettings(hostContext);
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
        static AppSettings BuildAppSettings(HostBuilderContext hostContext)
        {
            AppSettings settings = new();
            hostContext.Configuration.Bind("AppSettings", settings);
            if (settings.BotToken == "INCERT_TOKEN_HERE")
            {
                Console.WriteLine("You didnt adjust the AppSetting.json file, so input your bot token here:");
                settings.BotToken = Console.ReadLine();
            }
            settings.DownloadsFolder = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "TEMP")).FullName;
            return settings;
        }
    }
}