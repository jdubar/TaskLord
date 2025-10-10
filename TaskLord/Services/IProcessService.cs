using TaskLord.Enums;

namespace TaskLord.Services;

public interface IProcessService
{
    string ServiceName { get; }
    string TrayName { get; }

    Task<ServiceProcResult> StopProcessAsync(string name);
}
