using FakeItEasy;

using TaskLord.Enums;
using TaskLord.Services;

namespace TaskLordTests;

public class ProcessTests
{
    private IProcessService ProcessService { get; set; }

    public ProcessTests()
    {
        ProcessService = A.Fake<IProcessService>();
    }

    [Theory]
    [InlineData(ServiceProcResult.NoServiceFound)]
    [InlineData(ServiceProcResult.Success)]
    [InlineData(ServiceProcResult.Error)]
    public async Task FindProcess_ShouldReturn_ExpectedResult(ServiceProcResult expected)
    {
        // Given
        var name = "SomeProcess";
        A.CallTo(() => ProcessService.StopProcess(name)).Returns(Task.FromResult(expected));

        // When
        var actual = await ProcessService.StopProcess(name);

        // Then
        Assert.Equal(expected, actual);
    }
}