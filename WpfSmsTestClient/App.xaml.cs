using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Configuration;
using System.Data;
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

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"Logs/test-sms-wpf-app-{DateTime.Now:yyyyMMdd}.log", rollingInterval: RollingInterval.Day)
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