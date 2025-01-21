public class ConfigLoaderTests
{
    [Fact]
    public void LoadConfiguration_ShouldNotReturnNull()
    {
        // Act
        var config = ConfigLoader.LoadConfiguration();

        // Assert
        Assert.NotNull(config);
    }

}