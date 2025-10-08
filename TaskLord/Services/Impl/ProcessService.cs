using TaskLord.Enums;
using TaskLord.Utilities;

namespace TaskLord.Services.Impl;

public class ProcessService(IProcessWrapper process) : IProcessService
{
    public string ServiceName => TextUtility.ServiceName();
    public string TrayName => TextUtility.TrayName();

    public async Task<ServiceProcResult> StopProcess(string name)
    {
        var prc = process.GetProcessesByName(name).FirstOrDefault();
        if (prc is null)
        {
            return ServiceProcResult.NoServiceFound;
        }

        if (await process.KillAsync(prc))
        {
            return ServiceProcResult.Success;
        }
        else
        {
            return IsProcessForceStopped(prc.Id)
                ? ServiceProcResult.Success
                : ServiceProcResult.Error;
        }
    }

    private bool IsProcessForceStopped(int id)
    {
        process.Start(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"), $"/c taskkill /pid {id} /f");
        return process.GetProcessById(id) is null;
    }
}
