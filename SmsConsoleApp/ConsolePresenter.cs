using Microsoft.Extensions.Logging;

namespace SmsConsoleApp
{
    public class ConsolePresenter : IPresenter
    {
        private readonly ILogger _logger;

        public ConsolePresenter(ILogger logger) 
        {
            _logger = logger;
        }

        public void WriteLine(string message) 
        {
            Console.WriteLine(message);
            _logger.LogInformation(message);
        }
    }
}