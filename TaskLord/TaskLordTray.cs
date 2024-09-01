using TaskLord.Enums;
using TaskLord.Services;

namespace TaskLord;

public class TaskLordTray : ApplicationContext
{
    private readonly NotifyIcon _trayIcon;
    private readonly IProcessService _processService;

    public TaskLordTray(IProcessService processService)
    {
        _processService = processService;

        _trayIcon = new NotifyIcon()
        {
            Icon = Resources.Icon,
            ContextMenuStrip = new ContextMenuStrip()
            {
                Items = { new ToolStripMenuItem("Exit", null, Exit) }
            },
            Visible = true
        };
        _ = RunInBackground(TimeSpan.FromSeconds(10));
    }

    private async Task RunInBackground(TimeSpan timeSpan)
    {
        var processes = new[]
        {
            _processService.GetServiceText(),
            _processService.GetTrayText()
        };
        using var periodicTimer = new PeriodicTimer(timeSpan);
        while (await periodicTimer.WaitForNextTickAsync())
        {
            foreach (var process in processes)
            {
                switch (await _processService.StopProcess(process))
                {
                    case ServiceProcResult.Success:
                        _trayIcon.ShowBalloonTip(500, "Success", "Successfully stopped the service!", ToolTipIcon.Info);
                        break;
                    case ServiceProcResult.Error:
                        _trayIcon.ShowBalloonTip(500, "Error", "Unable to stop the service...", ToolTipIcon.Error);
                        break;
                    case ServiceProcResult.Unknown:
                    case ServiceProcResult.UnableToKill:
                    case ServiceProcResult.NoServiceFound:
                    default:
                        break;
                }
            }
        }
    }

    private void Exit(object? sender, EventArgs e)
    {
        _trayIcon.Visible = false;
        Application.Exit();
    }
}