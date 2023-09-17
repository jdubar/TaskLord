using TaskLord.Enums;

namespace TaskLord.Services;

public interface IProcessService
{
    Task<ServiceProcResult> StopProcess();
}
