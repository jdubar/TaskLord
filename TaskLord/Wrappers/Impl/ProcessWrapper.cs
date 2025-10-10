using System.Diagnostics;

namespace TaskLord.Wrappers.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No need to test wrapper code")]
public class ProcessWrapper : IProcess
{
    public bool ProcessIdExists(int id) => GetProcessById(id) is not null;

    public IEnumerable<int> GetProcessIdsByName(string name) => Process.GetProcessesByName(name).Select(p => p.Id);

    public async Task<bool> KillAsync(int id)
    {
        try
        {
            using var process = GetProcessById(id);
            process.Kill();
            await process.WaitForExitAsync();
            return process.HasExited;
        }
        catch (ArgumentException)
        {
            // Process with the specified ID does not exist.
            return true; // Consider it "killed" since it doesn't exist.
        }
        catch (Exception)
        {
            return false; // Indicate failure to kill the process.
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

    private static Process GetProcessById(int id) => Process.GetProcessById(id);
}
