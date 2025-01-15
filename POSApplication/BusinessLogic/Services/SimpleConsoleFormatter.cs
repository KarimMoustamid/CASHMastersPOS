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
        var logLevel = logEntry.LogLevel.ToString().ToUpper(); // Example: INFO, ERROR
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception)
                      ?? logEntry.State?.ToString();

        // Write formatted log message
        if (!string.IsNullOrWhiteSpace(message))
        {
            textWriter.WriteLine($"[{logLevel}] {message}");
        }
    }
}