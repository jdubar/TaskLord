namespace TaskLord.Wrappers.Impl;
public class SendKeysWrapper : ISendKeys
{
    const string DeleteAll = "^a{DEL}";
    public void SelectAllAndDelete() => SendKeys.SendWait(DeleteAll);
    public void SendWait(string keys) => SendKeys.SendWait(keys);
}
