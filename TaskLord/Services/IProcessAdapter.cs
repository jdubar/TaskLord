using System.Diagnostics;

namespace TaskLord.Services;
public interface IProcessAdapter
{
    Process? GetProcessInfo(string processName);
    bool IsProcessForceStopped(int id);
}
