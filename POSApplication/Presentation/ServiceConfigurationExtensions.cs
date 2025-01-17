namespace POSApplication.Presentation
{
    using BusinessLogic;
    using Data;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // DI Configuration
            services.AddSingleton<ICurrencyConfig, CurrencyConfig>(); // Currency configuration service
            services.AddTransient<ChangeCalculator>(); // Business logic service
            services.AddTransient<UserInteractionHelper>(); // Utilities for input
            services.AddTransient<ApplicationRunner>(); // Main application logic

            // Add Logging Configuration
            services.AddLogging(config =>
            {
                config.ClearProviders();
                config.AddConsole();
            });

            return services;
        }
    }
}