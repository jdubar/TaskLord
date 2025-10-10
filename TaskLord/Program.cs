using Microsoft.Extensions.Configuration;

using TaskLord.Models;
using TaskLord.Services.Impl;
using TaskLord.Wrappers.Impl;

namespace TaskLord;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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

            var sendKeysWrapper = new SendKeysWrapper();
            var processWrapper = new ProcessWrapper();
            var processService = new ProcessService(processWrapper);
            var automationService = new AutomationService(sendKeysWrapper, automationOptions);
            using var taskLordTray = new TaskLordTray(processService);

            automationService.AddAutomationEventHandler();
            Application.Run(taskLordTray);
            automationService.RemoveAllEventHandlers();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An unhandled exception occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}