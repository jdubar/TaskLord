using System.Diagnostics;

using TaskLord.Enums;
using TaskLord.Services;

namespace TaskLord;

public class TaskLordTray : ApplicationContext
{
    private readonly NotifyIcon TrayIcon;
    private readonly IProcessService _processService;

    public TaskLordTray(IProcessService processService)
    {
        _processService = processService;

        TrayIcon = new NotifyIcon()
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
        using var periodicTimer = new PeriodicTimer(timeSpan);
        while (await periodicTimer.WaitForNextTickAsync())
        {
            switch (await _processService.StopProcess())
            {
                case ServiceProcResult.Success:
                    TrayIcon.ShowBalloonTip(500, "Success", "Successfully stopped the service!", ToolTipIcon.Info);
                    break;
                case ServiceProcResult.Error:
                    TrayIcon.ShowBalloonTip(500, "Error", "Unable to stop the service...", ToolTipIcon.Error);
                    break;
                default:
                    break;
            }
        }
    }

    private void Exit(object? sender, EventArgs e)
    {
        TrayIcon.Visible = false;
        Application.Exit();
    }
}