using System.Diagnostics;
using System.IO;

using TaskLord.Enums;

namespace TaskLord.Services.Impl;
public class ProcessWrapper : IProcessWrapper, IDisposable
{
    public async Task<ServiceProcResult> StopProcess(string name)
    {
        using var process = GetProcessInfo(name);
        if (process is null)
        {
            return ServiceProcResult.NoServiceFound;
        }

        using (process)
        {
            try
            {
                process.Kill();
                await process.WaitForExitAsync();
                return ServiceProcResult.Success;
            }
            catch
            {
                return IsProcessForceStopped(process.Id)
                    ? ServiceProcResult.Success
                    : ServiceProcResult.Error;
            }
        }
    }

    private static Process GetProcessById(int id) => Process.GetProcessById(id);

    private static Process? GetProcessInfo(string processName) => Process.GetProcessesByName(processName).FirstOrDefault();

    private static bool IsProcessForceStopped(int id)
    {
        TaskKillProcessByPid(id);
        return GetProcessById(id) == null;
    }

    private static void TaskKillProcessByPid(int id)
    {
        _ = Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
            CreateNoWindow = true,
            UseShellExecute = false,
            Arguments = $"/c taskkill /pid {id} /f"
        });
    }

    public void Dispose() => GC.SuppressFinalize(this);
}
