using Xunit;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void SplitStorageTest()
    {
        string placeBackup = @"/home/";
        BackupObject obj1 = new BackupObject(@"/bin/chmod");
        BackupObject obj2 = new BackupObject(@"/bin/sleep");
        BackupTask task = new BackupTask("task3", placeBackup);
        Config config = new Config(new Backup(), new SplitSaver(), new InMemoryRepository(new MemoryFileSystem()));
        task.SetRepository(config.Repository);
        task.SetStorageAlgorithm(config.Algorithm);
        task.AddObject(obj1);
        task.AddObject(obj2);
        var points = new List<RestorePoint>();
        var point1 = task.AddPoint();
        points.Add(point1);
        task.RemoveObject(obj1);
        var point2 = task.AddPoint();
        points.Add(point2);
        Assert.True(points.Count == 2);
    }
}