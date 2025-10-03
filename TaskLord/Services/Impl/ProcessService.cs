using System.Text;

using TaskLord.Enums;

namespace TaskLord.Services.Impl;

public class ProcessService(IProcess process) : IProcessService, IDisposable
{
    private bool _disposed;

    public string ServiceName => GetServiceText();
    public string TrayName => GetTrayText();

    public async Task<ServiceProcResult> StopProcess(string name)
    {
        using var prc = process.GetProcessInfo(name);
        if (prc is null)
        {
            return ServiceProcResult.NoServiceFound;
        }

        using (prc)
        {
            try
            {
                prc.Kill();
                await prc.WaitForExitAsync();
                return ServiceProcResult.Success;
            }
            catch
            {
                return process.IsProcessForceStopped(prc.Id)
                    ? ServiceProcResult.Success
                    : ServiceProcResult.Error;
            }
        }
    }

    public string GetTrayText()
    {
        byte[] bytes = [68, 97, 116, 97, 78, 111, 119, 95, 84, 114, 97, 121];
        return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    }

    public string GetServiceText()
    {
        byte[] bytes = [68, 97, 116, 97, 78, 111, 119, 95, 83, 101, 114, 118, 105, 99, 101];
        return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    }

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
