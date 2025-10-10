namespace TaskLord.Services;
public interface IProcess
{
    bool ProcessIdExists(int id);
    IEnumerable<int> GetProcessIdsByName(string name);
    Task<bool> KillAsync(int id);
    void Start(string fileName, string arguments);
}
