using TaskLord.Services.Impl;

namespace TaskLord;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var process = new ProcessAdapter();
        var processService = new ProcessService(process);
        var taskLordTray = new TaskLordTray(processService);
        Application.Run(taskLordTray);
    }
}