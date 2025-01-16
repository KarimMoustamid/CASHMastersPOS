namespace POSApplication.Tests
{
    using BusinessLogic;
    using Data;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestPlatform.TestHost;
    using Moq;

    public class POSApplicationTests
    {
        private readonly ServiceCollection _services;
        private readonly Mock<ILogger<Program>> _mockLogger;
        private readonly Mock<ICurrencyConfig> _mockCurrencyConfig;
        private readonly Mock<UserInteractionHelper> _mockUserInteractionHelper;

        public POSApplicationTests()
        {
            // Mock dependencies
            _mockLogger = new Mock<ILogger<Program>>();
            _mockCurrencyConfig = new Mock<ICurrencyConfig>();
            _mockUserInteractionHelper = new Mock<UserInteractionHelper>(
                Mock.Of<ILogger<UserInteractionHelper>>(),
                new Mock<IServiceProvider>().Object
            );

            // Setup DI container for testing
            _services = new ServiceCollection();
            _services.AddSingleton(_mockCurrencyConfig.Object);
            _services.AddTransient(_ => _mockUserInteractionHelper.Object);
            _services.AddSingleton(_mockLogger.Object);
            _services.AddTransient<ChangeCalculator>();
        }

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
            Assert.NotNull(serviceProvider.GetService<ILogger<Program>>());
            Assert.NotNull(serviceProvider.GetService<UserInteractionHelper>());
        }

    }
}