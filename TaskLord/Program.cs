using TaskLord.Services.Impl;

namespace TaskLord;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var processService = new ProcessService();
        var taskLordTray = new TaskLordTray(processService);
        Application.Run(taskLordTray);
    }
}