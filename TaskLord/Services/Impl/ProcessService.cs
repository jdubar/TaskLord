using TaskLord.Enums;
using TaskLord.Utilities;

namespace TaskLord.Services.Impl;

public class ProcessService(IProcessWrapper process) : IProcessService, IDisposable
{
    private bool _disposed;

    public string ServiceName => TextUtility.ServiceName();
    public string TrayName => TextUtility.TrayName();

    public async Task<ServiceProcResult> StopProcess(string name) => await process.StopProcess(name);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            if (process is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        _disposed = true;
    }
}
