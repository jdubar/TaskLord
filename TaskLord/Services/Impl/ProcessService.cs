using System.Diagnostics;

using TaskLord.Enums;

namespace TaskLord.Services.Impl;

public class ProcessService : IProcessService
{
    private string ServiceText { get; } = "DataNow";

    public ServiceProcResult StopProcess()
    {
        var prc = Process.GetProcesses().ToList().Find(process => process.ProcessName.Contains(ServiceText));
        if (prc != null)
        {
            using (prc)
            {
                try
                {
                    prc.Kill();
                    return ServiceProcResult.Success;
                }
                catch
                {
                    return IsProcessForceStopped(prc.Id)
                        ? ServiceProcResult.Success
                        : ServiceProcResult.Error;
                }
            }
        }
        return ServiceProcResult.NoServiceFound;
    }

    private static bool IsProcessForceStopped(int id)
    {
        _ = Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            CreateNoWindow = true,
            UseShellExecute = false,
            Arguments = $"/c taskkill /pid {id} /f"
        });
        return !Process.GetProcesses().ToList().Exists(process => process.Id == id);
    }
}
