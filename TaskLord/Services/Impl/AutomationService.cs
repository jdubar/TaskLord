using System.Windows.Automation;

using TaskLord.Models;
using TaskLord.Wrappers;

namespace TaskLord.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "UI-based code cannot be easily tested")]
public class AutomationService(
    ISendKeys sendKeys,
    AutomationOptions options) : IAutomationService
{
    public void AddAutomationEventHandler()
        => Automation.AddAutomationEventHandler(eventId: WindowPattern.WindowOpenedEvent,
                                                element: AutomationElement.RootElement,
                                                scope: TreeScope.Children,
                                                eventHandler: OnWindowOpened);
    public void RemoveAllEventHandlers() => Automation.RemoveAllEventHandlers();

    private void OnWindowOpened(object sender, AutomationEventArgs automationEventArgs)
    {
        try
        {
            if (sender is not AutomationElement window || window.Current.Name != options.WindowTitle)
            {
                return;
            }

            var edit = GetWindowControl(window, options.EditControl);
            if (edit is null)
            {
                return;
            }

            if (edit.TryGetCurrentPattern(ValuePattern.Pattern, out object pattern))
            {
                ((ValuePattern)pattern).SetValue(options.TextToWrite);
                sendKeys.SendWait(".");
            }
            else
            {
                edit.SetFocus();
                sendKeys.SelectAllAndDelete();
                sendKeys.SendWait(options.TextToWrite);
            }

            var submit = GetWindowControl(window, options.ButtonControl);
            if (submit is not null)
            {
                var btnPattern = submit.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                btnPattern?.Invoke();
            }
        }
        catch (ElementNotAvailableException)
        {
            // Ignore exceptions to prevent crashing the application
        }
    }

    private static AutomationElement? GetWindowControl(AutomationElement window, KeyValuePair<int, string> control)
    {
        return window.FindAll(TreeScope.Subtree, Condition.TrueCondition)
                     .Cast<AutomationElement>()
                     .FirstOrDefault(e => e.Current.ControlType.LocalizedControlType == control.Value &&
                                          e.Current.ControlType.Id == control.Key);
    }
}
