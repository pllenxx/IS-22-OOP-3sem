using Backups.Extra.Recover;
using Backups.Extra.Remover;
using Xunit;
using Zio.FileSystems;

namespace Backups.Extra.Test;

public class BackupsExtraTests
{
    [Fact]
    public void ControlRestorePointsAmount()
    {
        string generalPath = @"/home/";
        BackupObject obj1 = new BackupObject(@"/bin/chmod");
        BackupObject obj2 = new BackupObject(@"/bin/date");
        BackupExtraTask task = new BackupExtraTask("task1", generalPath);
        task.SetTypeOfLogging(new ConsoleLogger(), false);
        var fs = new MemoryFileSystem();
        task.SetRepository(new InMemoryRepository(fs));
        task.SetStorageAlgorithm(new SplitSaver());
        task.AddObject(obj1);
        task.AddObject(obj2);
        task.AddPoint();
        BackupObject obj3 = new BackupObject(@"/bin/pwd");
        task.AddObject(obj3);
        task.AddPoint();
        task.SetRemovalPolicy(new AmountLimitAlgorithm(1));
        task.SetPolicyForSystemRemoval(new InMemoryRemover(), fs);
        task.ClearRestorePoints();
        Assert.Equal(1, task.Backup.GetPoints().Count);
    }
}