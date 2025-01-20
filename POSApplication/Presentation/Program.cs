using Microsoft.Extensions.Configuration; // For configuration-related operations.
using Microsoft.Extensions.DependencyInjection; // Used for Dependency Injection (DI) to wire services into the application.
using Microsoft.Extensions.Hosting; // Provides abstractions for a hosting environment.
using Microsoft.Extensions.Logging; // Provides logging capabilities for the application.
using Microsoft.Extensions.Logging.Console; // Supports console-based logging.
using POSApplication.BusinessLogic; // Imports business logic layer modules.
using POSApplication.BusinessLogic.config; // Imports configuration-related classes under the business logic.
using POSApplication.BusinessLogic.Utilities; // Imports utilities from the business logic.
using POSApplication.BusinessLogic.Utilities.Payments; // Imports payment utilities for handling payment-related tasks.
using POSApplication.Data; // Imports the data access layer.
using POSApplication.Data.Models; // Imports data models used in the application.
using POSApplication.Presentation; // Imports the presentation layer.
using POSApplication.Presentation.Utilities.logs; // Provides customized logging utilities for the presentation layer.

// Explicit Application Entry Point class
public class Program
{
    // The main entry point for the application
    public static void Main(string[] args)
    {
        // Create and configure a dependency injection service container.
        // ConfigureServices() is expected to be an extension method that registers all necessary services to the ServiceCollection.
        var serviceProvider = new ServiceCollection()
            .ConfigureServices()
            .BuildServiceProvider(); // Creates and builds the service provider to facilitate DI.

        // Resolving the logger instance using dependency injection.
        // ILogger<Program> is used to log details specific to the Program class.
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            // Resolving the application's runner class using dependency injection.
            // ApplicationRunner is responsible for running the main logic of the application.
            var app = serviceProvider.GetRequiredService<ApplicationRunner>();

            // Run the application. The ApplicationRunner is expected to execute the application's core processes.
            app.Run();
        }
        catch (Exception ex)
        {
            // Log an error if an exception occurs during application execution.
            // The logger logs the exception message with its stack trace.
            logger.LogError(ex, "An unexpected error occurred.");
        }
    }
}