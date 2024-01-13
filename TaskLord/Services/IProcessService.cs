using TaskLord.Enums;

namespace TaskLord.Services;

public interface IProcessService
{
    Task<ServiceProcResult> StopProcess(string name);
    string GetTrayText();
    string GetServiceText();
}
