namespace TaskLord.Models;
public record AutomationOptions
{
    /// <summary>
    /// The title of the window to interact with.
    /// </summary>
    public string WindowTitle { get; init; } = string.Empty;

    /// <summary>
    /// The text to write into the edit control.
    /// </summary>
    public string TextToWrite { get; init; } = string.Empty;

    /// <summary>
    /// The control type and ID of the button to click.
    /// </summary>
    public KeyValuePair<int, string> ButtonControl { get; init; } = new(50000, "button");

    /// <summary>
    /// The control type and ID of the edit control to write text into.
    /// </summary>
    public KeyValuePair<int, string> EditControl { get; init; } = new(50004, "edit");
}
