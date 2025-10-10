namespace TaskLord.Wrappers.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No need to test wrapper code")]
public class SendKeysWrapper : ISendKeys
{
    const string DeleteAll = "^a{DEL}";
    public void SelectAllAndDelete() => SendKeys.SendWait(DeleteAll);
    public void SendWait(string keys) => SendKeys.SendWait(keys);
}
