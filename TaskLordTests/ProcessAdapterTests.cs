using FakeItEasy;

using System.Diagnostics;

using TaskLord.Services;
using TaskLord.Services.Impl;

namespace TaskLordTests;
public class ProcessAdapterTests
{
    [Fact]
    public void GetProcessInfo_ShouldReturn_Process_WhenCalledWithValidName()
    {
        // Arrange
        var processName = "explorer.exe";
        var expected = new Process();
        var wrapper = A.Fake<IProcessWrapper>();
        var process = new ProcessAdapter(wrapper);
        A.CallTo(() => wrapper.GetProcessByName(processName)).Returns(expected);

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
        var wrapper = A.Fake<IProcessWrapper>();
        var process = new ProcessAdapter(wrapper);
        A.CallTo(() => wrapper.GetProcessByName(processName)).Returns(null);

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
        var wrapper = A.Fake<IProcessWrapper>();
        var process = new ProcessAdapter(wrapper);
        A.CallTo(() => wrapper.GetProcessById(processId))!.Returns(null);

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
        var wrapper = A.Fake<IProcessWrapper>();
        var process = new ProcessAdapter(wrapper);
        A.CallTo(() => wrapper.GetProcessById(processId)).Returns(new Process());

        // Act
        var actual = process.IsProcessForceStopped(processId);

        // Assert
        Assert.False(actual);
    }
}
