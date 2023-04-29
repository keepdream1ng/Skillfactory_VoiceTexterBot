using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTexterBotApp.Controllers;
using VoiceTexterBotApp.Controllers;

namespace VoiceTexterBotApp
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Input token for bot.");
            string _token = Console.ReadLine();


            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services, _token)) // Задаем конфигурацию
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Service is started.");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Service is stopped.");
        }

        static void ConfigureServices(IServiceCollection services, string token)
        {
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();


            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(token));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
    }
}