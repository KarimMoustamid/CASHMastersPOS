using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using System.IO;
using Microsoft.Extensions.Logging;

public class SimpleConsoleFormatter : ConsoleFormatter
{
    public SimpleConsoleFormatter() : base("Simple")
    {
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        // Extract the log level and message
        var logLevel = logEntry.LogLevel;
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception)
                      ?? logEntry.State?.ToString();

        // Write formatted log message
        if (!string.IsNullOrWhiteSpace(message))
        {
            // Set console color based on log level
            SetConsoleColor(logLevel);

            // Write log message
            textWriter.WriteLine($"[{logLevel}] {message}");

            // Reset console color
            Console.ResetColor();
        }
    }

    // TODO : TEST METHOD
    private void SetConsoleColor(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
                Console.ForegroundColor = ConsoleColor.Gray; // Light color for less critical logs
                break;
            case LogLevel.Information:
                Console.ForegroundColor = ConsoleColor.Green; // Green for information
                break;
            case LogLevel.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow; // Yellow for warnings
                break;
            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.Red; // Red for errors
                break;
            case LogLevel.Critical:
                Console.ForegroundColor = ConsoleColor.Magenta; // Magenta for critical errors
                break;
            default:
                Console.ResetColor(); // Default color for unspecified log levels
                break;
        }
    }
}