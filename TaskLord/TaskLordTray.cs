using TaskLord.Enums;
using TaskLord.Services;

namespace TaskLord;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class TaskLordTray : ApplicationContext, IDisposable
{
    private readonly NotifyIcon _trayIcon;
    private readonly IProcessService _processService;
    private readonly CancellationTokenSource _cts = new();
    private readonly Task? _backgroundTask;

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
            Text = "Service is running...",
            Visible = true
        };

        _backgroundTask = RunInBackground(TimeSpan.FromSeconds(10), _cts.Token);
    }

    private async Task RunInBackground(TimeSpan timeSpan, CancellationToken cancellationToken)
    {
        var processes = new[]
        {
            _processService.ServiceName,
            _processService.TrayName
        };

        using var periodicTimer = new PeriodicTimer(timeSpan);
        try
        {
            while (await periodicTimer.WaitForNextTickAsync(cancellationToken))
            {
                foreach (var process in processes)
                {
                    if (await _processService.StopProcessAsync(process) is ServiceProcResult.UnableToKill)
                    {
                        _trayIcon.ShowBalloonTip(500, "Error", $"Unable to stop {process}", ToolTipIcon.Error);
                        break;
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when the token is canceled
        }
        catch (Exception ex)
        {
            _trayIcon.ShowBalloonTip(500, "Error", $"Background error: {ex.Message}", ToolTipIcon.Error);
        }
    }

    private void Exit(object? sender, EventArgs e)
    {
        _cts.Cancel();
        _trayIcon.Visible = false;
        Application.Exit();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _trayIcon.Dispose();
            _cts.Dispose();
            _backgroundTask?.Dispose();
        }
        base.Dispose(disposing);
    }
}
