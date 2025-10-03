using System.Windows.Automation;

using TaskLord.Models;

namespace TaskLord.Services.Impl;
public class AutomationService(AutomationOptions options) : IAutomationService, IDisposable
{
    public void RemoveAllEventHandlers() => Automation.RemoveAllEventHandlers();

    public void AddAutomationEventHandler()
        => Automation.AddAutomationEventHandler(eventId: WindowPattern.WindowOpenedEvent,
                                                element: AutomationElement.RootElement,
                                                scope: TreeScope.Children,
                                                eventHandler: OnWindowOpened);

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
                SendKeys.SendWait(".");
            }
            else
            {
                edit.SetFocus();
                SendKeys.SendWait("^{HOME}"); // Move to start of control
                SendKeys.SendWait("^+{END}"); // Select all text
                SendKeys.SendWait("{DEL}");   // Delete selected text
                SendKeys.SendWait(options.TextToWrite);
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

    public void Dispose()
    {
        RemoveAllEventHandlers();
        GC.SuppressFinalize(this);
    }
}
