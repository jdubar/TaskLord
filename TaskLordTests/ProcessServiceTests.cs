using FakeItEasy;

using System.Diagnostics;

using TaskLord.Enums;
using TaskLord.Services;
using TaskLord.Services.Impl;

namespace TaskLordTests;

public class ProcessServiceTests
{
    [Fact]
    public async Task StopProcess_ShouldReturn_NoServiceFound_WhenEmptyArrayIsReturned()
    {
        // Arrange
        var process = "SomeProcess";
        var expected = ServiceProcResult.NoServiceFound;
        var wrapper = A.Fake<IProcessWrapper>();
        var service = new ProcessService(wrapper);
        A.CallTo(() => wrapper.GetProcessesByName(A<string>._)).Returns([]);

        // Act
        var actual = await service.StopProcess(process);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task StopProcess_ShouldReturn_Success_WhenProcessIsKilled()
    {
        // Arrange
        Process[] processes = [A.Fake<Process>()];
        var process = "SomeProcess";
        var expected = ServiceProcResult.Success;
        var wrapper = A.Fake<IProcessWrapper>();
        var service = new ProcessService(wrapper);
        A.CallTo(() => wrapper.GetProcessesByName(A<string>._)).Returns(processes);
        A.CallTo(() => wrapper.KillAsync(A<Process>._)).Returns(true);

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
}
