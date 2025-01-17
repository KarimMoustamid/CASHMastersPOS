using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using POSApplication.BusinessLogic;
using POSApplication.BusinessLogic.config;
using POSApplication.BusinessLogic.Utilities;
using POSApplication.BusinessLogic.Utilities.Payments;
using POSApplication.Data;
using POSApplication.Data.Models;
using POSApplication.Presentation;
using POSApplication.Presentation.Utilities.logs;

// Explicit Application Entry Point :
public class Program
{
    public static void Main(string[] args)
    {
        // Configure services using extension method
        var serviceProvider = new ServiceCollection()
            .ConfigureServices()
            .BuildServiceProvider();

        // Resolve logger
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();


        try
        {
            // Run the application
            var app = serviceProvider.GetRequiredService<ApplicationRunner>();
            app.Run();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
        }
    }
}