namespace POSApplication.Tests
{
    using BusinessLogic;
    using Data;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Presentation.Utilities.logs;

    public class DependencyInjectionTests
    {
        [Fact]
        public void Services_Should_Register_All_Dependencies()
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddSingleton<ICurrencyConfig, CurrencyConfig>();
            services.AddTransient<ChangeCalculator>();
            services.AddLogging(config =>
            {
                config.ClearProviders();
                config.AddConsole();
            });
            services.AddTransient<UserInteractionHelper>();

            // Act
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            Assert.NotNull(serviceProvider.GetService<ICurrencyConfig>());
            Assert.NotNull(serviceProvider.GetService<ChangeCalculator>());
            Assert.NotNull(serviceProvider.GetService<ILogger<object>>());
            Assert.NotNull(serviceProvider.GetService<UserInteractionHelper>());
        }
    }
}