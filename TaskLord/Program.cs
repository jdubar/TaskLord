using Microsoft.Extensions.Configuration;

using TaskLord.Models;
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

        try
        {
            var config = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                         .AddUserSecrets(typeof(Program).Assembly)
                         .Build();
            var automationOptions = config.GetSection(nameof(AutomationOptions))
                                          .Get<AutomationOptions>();

            if (automationOptions is null)
            {
                MessageBox.Show("Unable to load configuration. Exiting...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var automation = new AutomationService(automationOptions);
            using var process = new ProcessAdapter();
            using var processService = new ProcessService(process);
            using var taskLordTray = new TaskLordTray(processService);

            automation.AddAutomationEventHandler();
            Application.Run(taskLordTray);
            automation.RemoveAllEventHandlers();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An unhandled exception occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}