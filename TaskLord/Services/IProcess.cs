using System.Diagnostics;

namespace TaskLord.Services;
public interface IProcess
{
    Process? GetProcessInfo(string processName);
    bool IsProcessForceStopped(int id);
}
