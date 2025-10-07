using System.Diagnostics;

namespace TaskLord.Services.Impl;
public class ProcessAdapter(IProcessWrapper process) : IProcessAdapter, IDisposable
{
    public Process? GetProcessInfo(string processName) => process.GetProcessByName(processName);

    public bool IsProcessForceStopped(int id)
    {
        process.KillProcessByPid(id);

        return process.GetProcessById(id) == null;
    }

    public void Dispose() => GC.SuppressFinalize(this);
}
