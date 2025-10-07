using FakeItEasy;

using TaskLord.Enums;
using TaskLord.Services;
using TaskLord.Services.Impl;

namespace TaskLordTests;

public class ProcessServiceTests
{
    [Theory]
    [InlineData(ServiceProcResult.NoServiceFound)]
    [InlineData(ServiceProcResult.Success)]
    [InlineData(ServiceProcResult.Error)]
    public async Task StopProcess_ShouldReturn_ExpectedResult(ServiceProcResult expected)
    {
        // Arrange
        var process = "SomeProcess";
        var wrapper = A.Fake<IProcessWrapper>();
        var service = new ProcessService(wrapper);
        A.CallTo(() => wrapper.StopProcess(process)).Returns(expected);

        // Act
        var actual = await service.StopProcess(process);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ServiceName_ShouldReturn_ExpectedValue()
    {
        // Arrange
        var expected = "DataNow_Service";
        var process = A.Fake<IProcessWrapper>();
        var service = new ProcessService(process);

        // Act
        var actual = service.ServiceName;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void TrayName_ShouldReturn_ExpectedValue()
    {
        // Arrange
        var expected = "DataNow_Tray";
        var process = A.Fake<IProcessWrapper>();
        var service = new ProcessService(process);

        // Act
        var actual = service.TrayName;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Dispose_ShouldNotThrowException()
    {
        // Arrange
        var process = A.Fake<IProcessWrapper>();
        var service = new ProcessService(process);

        // Act & Assert
        var exception = Record.Exception(service.Dispose);
        Assert.Null(exception);
    }
}
