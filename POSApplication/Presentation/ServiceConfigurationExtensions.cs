namespace POSApplication.Presentation
{
    // Importing necessary namespaces
    using BusinessLogic; // Imports business logic layer for core application functionality.
    using Data; // Imports data-related configurations and models.
    using Microsoft.Extensions.DependencyInjection; // Enables Dependency Injection (DI) configuration.
    using Microsoft.Extensions.Logging; // Provides logging capabilities to the application.

    // A static class containing extension methods to configure services for the application.
    public static class ServiceConfigurationExtensions
    {
        // An extension method to configure services for dependency injection.
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Dependency Injection Configuration

            // Registering an implementation of the ICurrencyConfig interface.
            // The service is registered as a singleton, ensuring only one instance is used across the application.
            services.AddSingleton<ICurrencyConfig, CurrencyConfig>(); // Currency configuration service.

            // Registering the ChangeCalculator class.
            // It is added as a transient service, meaning a new instance is created each time it is requested.
            services.AddTransient<ChangeCalculator>(); // Business logic service for calculating change.

            // Registering the UserInteractionHelper class to handle user input/output utilities.
            // It is added as a transient service for reusable, stateless helper methods.
            services.AddTransient<UserInteractionHelper>(); // Utilities for user interaction.

            // Registering the ApplicationRunner class, which is responsible for running the main application logic.
            // This is also added as a transient service for flexibility.
            services.AddTransient<ApplicationRunner>(); // Core application execution logic.

            // Adding Logging Configuration
            services.AddLogging(config =>
            {
                // Clears any default logging providers that might have been added earlier.
                config.ClearProviders();

                // Adds a console logging provider, enabling the application to log to the console.
                config.AddConsole();
            });

            // Returning the configured IServiceCollection object.
            // This allows chaining of methods if needed and ensures the services are registered to the DI container.
            return services;
        }
    }
}