using System.Diagnostics;

using TaskLord.Enums;

namespace TaskLord.Services.Impl;

public class ProcessService : IProcessService
{
    private string ServiceText { get; } = "DataNow_Tray";

    public async Task<ServiceProcResult> StopProcess()
    {
        using var prc = Process.GetProcessesByName(ServiceText).FirstOrDefault();
        if (prc != null)
        {
            try
            {
                prc.Kill();
                await prc.WaitForExitAsync();
                return ServiceProcResult.Success;
            }
            catch
            {
                return IsProcessForceStopped(prc.Id)
                    ? ServiceProcResult.Success
                    : ServiceProcResult.Error;
            }
        }
        return ServiceProcResult.NoServiceFound;
    }

    private static bool IsProcessForceStopped(int id)
    {
        _ = Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
            CreateNoWindow = true,
            UseShellExecute = false,
            Arguments = $"/c taskkill /pid {id} /f"
        });
        return !Process.GetProcesses().ToList().Exists(process => process.Id == id);
    }
}
