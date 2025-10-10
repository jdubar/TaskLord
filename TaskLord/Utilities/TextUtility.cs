using System.Text;

namespace TaskLord.Utilities;
public static class TextUtility
{
    public static string TrayName()
    {
        byte[] bytes = [68, 97, 116, 97, 78, 111, 119, 95, 84, 114, 97, 121];
        return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    }

    public static string ServiceName()
    {
        byte[] bytes = [68, 97, 116, 97, 78, 111, 119, 95, 83, 101, 114, 118, 105, 99, 101];
        return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    }
}
