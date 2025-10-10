using FakeItEasy;

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
        var process = "Zelda.exe";
        var expected = ServiceProcResult.NoServiceFound;
        var wrapper = A.Fake<IProcess>();
        var service = new ProcessService(wrapper);

        A.CallTo(() => wrapper.GetProcessIdsByName(A<string>._)).Returns([]);

        // Act
        var actual = await service.StopProcessAsync(process);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task StopProcess_ShouldReturn_Success_WhenProcessIsKilled()
    {
        // Arrange
        IEnumerable<int> ids = [42];
        var process = "Link.exe";
        var expected = ServiceProcResult.Success;
        var wrapper = A.Fake<IProcess>();
        var service = new ProcessService(wrapper);

        A.CallTo(() => wrapper.GetProcessIdsByName(A<string>._)).Returns(ids);
        A.CallTo(() => wrapper.KillAsync(A<int>._)).Returns(true);

        // Act
        var actual = await service.StopProcessAsync(process);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task StopProcess_ShouldReturn_Success_WhenProcessKillFailsButIsSuccessfullyForceStopped()
    {
        // Arrange
        IEnumerable<int> ids = [42];
        var process = "Epona.exe";
        var expected = ServiceProcResult.Success;
        var wrapper = A.Fake<IProcess>();
        var service = new ProcessService(wrapper);

        A.CallTo(() => wrapper.GetProcessIdsByName(A<string>._)).Returns(ids);
        A.CallTo(() => wrapper.KillAsync(A<int>._)).Returns(false);
        A.CallTo(() => wrapper.Start(A<string>._, A<string>._)).DoesNothing();
        A.CallTo(() => wrapper.ProcessIdExists(A<int>._)).Returns(false);

        // Act
        var actual = await service.StopProcessAsync(process);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task StopProcess_ShouldReturn_Error_WhenProcessKillFailsAndForceStopFails()
    {
        // Arrange
        IEnumerable<int> ids = [42];
        var processName = "Ganon.exe";
        var expected = ServiceProcResult.UnableToKill;
        var wrapper = A.Fake<IProcess>();
        var service = new ProcessService(wrapper);

        A.CallTo(() => wrapper.GetProcessIdsByName(A<string>._)).Returns(ids);
        A.CallTo(() => wrapper.KillAsync(A<int>._)).Returns(false);
        A.CallTo(() => wrapper.Start(A<string>._, A<string>._)).DoesNothing();
        A.CallTo(() => wrapper.ProcessIdExists(A<int>._)).Returns(true);

        // Act
        var actual = await service.StopProcessAsync(processName);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ServiceName_ShouldReturn_ExpectedValue()
    {
        // Arrange
        var expected = "DataNow_Service";
        var process = A.Fake<IProcess>();
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
        var process = A.Fake<IProcess>();
        var service = new ProcessService(process);

        // Act
        var actual = service.TrayName;

        // Assert
        Assert.Equal(expected, actual);
    }
}
