using System.Diagnostics;

namespace TaskLord.Services.Impl;
public class ProcessAdapter : IProcess
{
    public Process? GetProcessInfo(string processName) =>
        Process.GetProcessesByName(processName).FirstOrDefault();

    public bool IsProcessForceStopped(int id)
    {
        _ = Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
            CreateNoWindow = true,
            UseShellExecute = false,
            Arguments = $"/c taskkill /pid {id} /f"
        });
        return Process.GetProcessById(id) == null;
    }
}
