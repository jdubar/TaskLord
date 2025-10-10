using TaskLord.Enums;
using TaskLord.Utilities;
using TaskLord.Wrappers;

namespace TaskLord.Services.Impl;

public class ProcessService(IProcess process) : IProcessService
{
    public string ServiceName => TextUtility.ServiceName();
    public string TrayName => TextUtility.TrayName();

    public async Task<ServiceProcResult> StopProcessAsync(string name)
    {
        var id = process.GetProcessIdsByName(name).FirstOrDefault();
        if (id == 0)
        {
            return ServiceProcResult.NoServiceFound;
        }

        if (await process.KillAsync(id))
        {
            return ServiceProcResult.Success;
        }

        return ProcessIsForceStopped(id)
            ? ServiceProcResult.Success
            : ServiceProcResult.UnableToKill;
    }

    private bool ProcessIsForceStopped(int id)
    {
        process.Start(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"), $"/c taskkill /pid {id} /f");
        return !process.ProcessIdExists(id);
    }
}
