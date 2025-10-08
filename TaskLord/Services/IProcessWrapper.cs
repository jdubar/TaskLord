using System.Diagnostics;

namespace TaskLord.Services;
public interface IProcessWrapper
{
    Process GetProcessById(int id);
    Process[] GetProcessesByName(string processName);
    Task<bool> KillAsync(Process process);
    void Start(string fileName, string arguments);
}
