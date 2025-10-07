using FakeItEasy;

using TaskLord.Enums;
using TaskLord.Services;

namespace TaskLordTests;

public class ProcessServiceTests
{
    [Theory]
    [InlineData(ServiceProcResult.NoServiceFound)]
    [InlineData(ServiceProcResult.Success)]
    [InlineData(ServiceProcResult.Error)]
    public async Task FindProcess_ShouldReturn_ExpectedResult(ServiceProcResult expected)
    {
        // Arrange
        var process = "SomeProcess";
        var service = A.Fake<IProcessService>();
        A.CallTo(() => service.StopProcess(process)).Returns(Task.FromResult(expected));

        // Act
        var actual = await service.StopProcess(process);

        // Assert
        Assert.Equal(expected, actual);
    }
}