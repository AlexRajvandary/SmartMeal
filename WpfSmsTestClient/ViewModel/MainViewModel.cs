using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfSmsTestClient.Commands;
using WpfSmsTestClient.Model;
using WpfSmsTestClient.Services;

namespace WpfSmsTestClient.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEnvironmentNotifier _envirnomentNotifier;
        private readonly ILogger<MainViewModel> _logger;
        private readonly IMessageBoxService _messageBoxService;
       

        private ObservableCollection<EnvironmentVariableModel> _environmentVariables;

        public MainViewModel(IConfiguration configuration,
                             IEnvironmentNotifier envirnomentNotifier,
                             ILogger<MainViewModel> logger,
                             IMessageBoxService messageBoxService)
        {
            _configuration = configuration;
            _envirnomentNotifier = envirnomentNotifier;
            _logger = logger;
            _messageBoxService = messageBoxService;
            
            EnvironmentVariables = new ObservableCollection<EnvironmentVariableModel>();

            SaveCommand = new RelayCommand(SaveEnvironmentVariables);
            LoadEnvironmentVariables();
        }

        public ObservableCollection<EnvironmentVariableModel> EnvironmentVariables
        {
            get => _environmentVariables;
            set => SetProperty(ref _environmentVariables, value);
        }

        public ICommand SaveCommand { get; }

        public void SaveEnvironmentVariables()
        {
            foreach (var variable in EnvironmentVariables)
            {
                try
                {
                    _logger.LogInformation($"Переменная {variable.Name} изменена. Новое значение: {variable.Value}");
                    Registry.SetValue(@"HKEY_CURRENT_USER\Environment", variable.Name, variable.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ошибка при записи переменной {variable.Name}: {ex.Message}");
                }
            }

            _envirnomentNotifier.NotifyEnvironmentChanged();

            _messageBoxService.ShowNotification("Данные сохранены!");
        }

        private void LoadEnvironmentVariables()
        {
            var systemEnvVariablesDict = Environment.GetEnvironmentVariables()!
                                            .Cast<System.Collections.DictionaryEntry>()!
                                            .Where(entry => entry.Value is string)!
                                            .ToDictionary(entry => entry.Key.ToString(),
                                                          entry => entry.Value?.ToString(), StringComparer.OrdinalIgnoreCase);

            var configVariables = _configuration.GetSection("EnvironmentVariables")
                .GetChildren()
                .Select(setting => new
                {
                    Name = setting.Key,
                    ConfigValue = setting.Value
                })
                .ToList();

            foreach (var configVar in configVariables)
            {
                if (systemEnvVariablesDict.TryGetValue(configVar.Name, out var systemValue))
                {
                    EnvironmentVariables.Add(new EnvironmentVariableModel
                    {
                        Name = configVar.Name,
                        Value = systemValue,
                        Comment = "Системная переменная"
                    });
                }
                else
                {
                    EnvironmentVariables.Add(new EnvironmentVariableModel
                    {
                        Name = configVar.Name,
                        Value = configVar.ConfigValue,
                        Comment = "Переменная отсутствует в системе, используется значение по умолчанию"
                    });
                }
            }
        }

    }
}