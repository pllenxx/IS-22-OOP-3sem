using Zio.FileSystems;

namespace Backups;

public class Program
{
    private static void Main()
    {
        string generalPath = @"/Users/khartanovichp/Desktop/forbackup";
        BackupObject obj1 = new BackupObject(@"/Users/khartanovichp/Desktop/file1.txt");
        BackupObject obj2 = new BackupObject(@"/Users/khartanovichp/Desktop/file2.txt");
        BackupTask task = new BackupTask("task1", generalPath);
        task.SetRepository(new FileSystemRepository(new PhysicalFileSystem()));
        task.SetStorageAlgorithm(new SingleSaver());
        task.AddObject(obj1);
        task.AddObject(obj2);
        task.AddPoint();
    }
}