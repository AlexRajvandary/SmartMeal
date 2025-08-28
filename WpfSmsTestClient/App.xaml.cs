using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using WpfSmsTestClient.Services;
using WpfSmsTestClient.ViewModel;

namespace WpfSmsTestClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var serviceCollection = new ServiceCollection();

                ConfigureServices(serviceCollection, configuration);

                ServiceProvider = serviceCollection.BuildServiceProvider();

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Ошибка при запуске приложения");
                MessageBox.Show("Ошибка запуска: " + ex.Message);
                Shutdown();
            }
        }


        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<IEnvironmentNotifier,  EnvironmentNotifier>();

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
                .WriteTo.File(Path.Combine(logFolder, $"test-sms-wpf-app-{DateTime.Now:yyyyMMdd}.log"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddSerilog();
            });
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
        }
    }
}