using System.Diagnostics;

namespace TaskLord.Services;
public interface IProcessWrapper
{
    Process? GetProcessByName(string processName);
    void KillProcessByPid(int id);
    Process GetProcessById(int id);
}
