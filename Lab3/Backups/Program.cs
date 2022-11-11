using Backups.Services;
using Zio;
namespace Backups;

public class Program
{
    private static void Main()
    {
        var path = "/Users/khartanovichp/Desktop/tmp";
        var content = File.ReadAllText(Path.Combine(path, "file1.txt"));
        File.WriteAllText(Path.Combine(path, "file.txt"), content);
    }
}