using TaskLord.Enums;

namespace TaskLord.Services;

public interface IProcessService
{
    Task<ServiceProcResult> StopProcess(string name);
    string ServiceName { get; }
    string TrayName { get; }
}
