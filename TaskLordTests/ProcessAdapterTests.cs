using FakeItEasy;

using System.Diagnostics;

using TaskLord.Services;

namespace TaskLordTests;
public class ProcessAdapterTests
{
    [Fact]
    public void GetProcessInfo_ShouldReturn_Process_WhenCalledWithValidName()
    {
        // Arrange
        var processName = "explorer.exe";
        var expected = new Process();
        var process = A.Fake<IProcessAdapter>();
        A.CallTo(() => process.GetProcessInfo(processName)).Returns(expected);

        // Act
        var actual = process.GetProcessInfo(processName);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsProcessForceStopped_ShouldReturn_Null_WhenCalledWithInvalidName()
    {
        // Arrange
        var processName = "invalid_process";
        var process = A.Fake<IProcessAdapter>();
        A.CallTo(() => process.GetProcessInfo(processName)).Returns(null);

        // Act
        var actual = process.GetProcessInfo(processName);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void IsProcessForceStopped_ShouldReturn_True_WhenProcessIsStopped()
    {
        // Arrange
        var processId = 1234; // Example process ID
        var process = A.Fake<IProcessAdapter>();
        A.CallTo(() => process.IsProcessForceStopped(processId)).Returns(true);

        // Act
        var actual = process.IsProcessForceStopped(processId);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsProcessForceStopped_ShouldReturn_False_WhenProcessIsNotStopped()
    {
        // Arrange
        var processId = 5678; // Example process ID
        var process = A.Fake<IProcessAdapter>();
        A.CallTo(() => process.IsProcessForceStopped(processId)).Returns(false);

        // Act
        var actual = process.IsProcessForceStopped(processId);

        // Assert
        Assert.False(actual);
    }
}
