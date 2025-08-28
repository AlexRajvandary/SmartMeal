using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using SmartMealApiClient.Services;
using System.Reflection;

namespace SmsConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string? connectionString = config.GetConnectionString("DefaultConnection");

            string? apiUrl = config["ApiUrl"];

            bool useGrpc = bool.TryParse(config["UseGrpc"], out var result) && result;

            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDir = Path.GetDirectoryName(exePath)!;

            string solutionDir = exeDir;
            for (int i = 0; i < 3; i++)
            {
                solutionDir = Path.GetDirectoryName(solutionDir)!;
            }

            string logFolder = Path.Combine(solutionDir, "logs");

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logFolder, $"test-sms-console-app-{DateTime.Now:yyyyMMdd}.log"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger<Program>();
            var presenter = new ConsolePresenter(logger);

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Ошибка: строка подключения к базе данных не указана в конфигурации.");
                return;
            }

            if (string.IsNullOrEmpty(apiUrl))
            {
                Console.WriteLine("Ошибка: URL API не указан в конфигурации.");
                return;
            }

            Console.WriteLine($"Подключение к базе данных: {connectionString}");
            Console.WriteLine($"API URL: {apiUrl}");

            var dbInitializer = new DatabaseInitializer(connectionString);
            dbInitializer.InitializeDatabase();

            //Не совсем понял как быть: в первой части первого задания сказано определить классы блюда и заказа, а во второй части дан .proto-файл
            //Решил для http создать свои классы, а для rpc сгенерировать аналогичные из файла
            if (!useGrpc)
            {
                var apiClient = new HttpApiClient(apiUrl, "username", "password");
                var menuFetcher = new MenuFetcher(apiClient, presenter);
                var menuItems = await menuFetcher.GetMenu();

                var menuSaver = new MenuSaver(connectionString, presenter);
                menuSaver.SaveMenuToDatabase(menuItems);
                menuSaver.DisplayMenu(menuItems);

                var orderInputHandler = new OrderInputHandler(menuItems, presenter);
                var orderItems = orderInputHandler.GetOrderFromUser();

                var orderSender = new OrderSender(apiClient, presenter);
                await orderSender.SendOrder(orderItems);
            }
            else
            {
                var apiClient = new GrpcApiClient(apiUrl);
                var menuFetcher = new MenuFetcher(apiClient, presenter);
                var menuItems = await menuFetcher.GetMenu();

                var menuSaver = new MenuSaver(connectionString, presenter);
                menuSaver.SaveMenuToDatabase(menuItems);
                menuSaver.DisplayMenu(menuItems);

                var orderInputHandler = new OrderInputHandler(menuItems, presenter);
                var orderItems = orderInputHandler.GetOrderFromUser();

                var orderSender = new OrderSender(apiClient);
                await orderSender.SendOrder(orderItems);
            }
        }
    }
}