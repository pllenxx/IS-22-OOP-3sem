using Xunit;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void SplitStorageTest()
    {
        // string generalPath = @"/Users/khartanovichp/Desktop/forbackup";
        // BackupObject obj1 = new BackupObject(@"/Users/khartanovichp/Desktop/file1.txt");
        // BackupObject obj2 = new BackupObject(@"/Users/khartanovichp/Desktop/file2.txt");
        // Config config = new Config(new Backup(), new SplitSaver(), new FileSystemRepository(new PhysicalFileSystem()));
        // BackupTask task = new BackupTask("task1", generalPath);
        // task.SetRepository(new FileSystemRepository(new PhysicalFileSystem()));
        // task.SetStorageAlgorithm(new SplitSaver());
        // task.AddObject(obj1);
        // task.AddObject(obj2);
        // var point1 = task.AddPoint();
        // task.RemoveObject(obj2);
        // var point2 = task.AddPoint();
        // var points = new List<RestorePoint>() { point1, point2 };
        // Assert.True(points.Count == 2);
        string placeBackup = @"/home/";
        BackupObject obj = new BackupObject(@"/bin/echo");
        BackupTask task = new BackupTask("task2", placeBackup);
        Config config = new Config(new Backup(), new SplitSaver(), new InMemoryRepository(new MemoryFileSystem()));
        task.SetRepository(config.Repository);
        task.SetStorageAlgorithm(config.Algorithm);
        task.AddObject(obj);
        task.AddPoint();
    }
}