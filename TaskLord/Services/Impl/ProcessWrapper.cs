using System.Diagnostics;

namespace TaskLord.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No need to test wrapper code")]
public class ProcessWrapper : IProcessWrapper
{
    public Process GetProcessById(int id) => Process.GetProcessById(id);

    public Process[] GetProcessesByName(string processName) => Process.GetProcessesByName(processName);

    public async Task<bool> KillAsync(Process process)
    {
        try
        {
            process.Kill();
            await process.WaitForExitAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void Start(string fileName, string arguments)
    {
        _ = Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            CreateNoWindow = true,
            UseShellExecute = false
        });
    }
}
