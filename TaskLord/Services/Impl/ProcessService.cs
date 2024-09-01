using System.Text;

using TaskLord.Enums;

namespace TaskLord.Services.Impl;

public class ProcessService(IProcess process) : IProcessService
{
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
}
