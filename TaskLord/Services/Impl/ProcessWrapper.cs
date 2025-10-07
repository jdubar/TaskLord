using System.Diagnostics;
using System.IO;

namespace TaskLord.Services.Impl;
public class ProcessWrapper : IProcessWrapper, IDisposable
{
    public Process? GetProcessByName(string processName) => Process.GetProcessesByName(processName).FirstOrDefault();

    public void KillProcessByPid(int id)
    {
        _ = Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
            CreateNoWindow = true,
            UseShellExecute = false,
            Arguments = $"/c taskkill /pid {id} /f"
        });
    }

    public Process GetProcessById(int id) => Process.GetProcessById(id);

    public void Dispose() => GC.SuppressFinalize(this);
}
