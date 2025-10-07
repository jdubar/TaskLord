using TaskLord.Enums;

namespace TaskLord.Services;
public interface IProcessWrapper
{
    Task<ServiceProcResult> StopProcess(string name);
}
