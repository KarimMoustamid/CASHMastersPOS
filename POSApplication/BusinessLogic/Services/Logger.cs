namespace POSApplication.BusinessLogic.Services
{
    using Microsoft.Extensions.Logging;

    public class Logger
    {
        private readonly ILogger<Logger> _logger;

        public Logger(ILogger<Logger> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            _logger.LogInformation(message);
            Console.ResetColor();
        }

        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.LogWarning(message);
            Console.ResetColor();
        }

        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            _logger.LogError(message);
            Console.ResetColor();
        }

        public void LogCritical(string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            _logger.LogCritical(message);
            Console.ResetColor();
        }
    }
}