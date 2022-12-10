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
        BackupObject obj1 = new BackupObject(@"/bin/kill");
        BackupObject obj2 = new BackupObject(@"/bin/ed");
        BackupExtraTask task1 = new BackupExtraTask("task 1", generalPath);
        task1.SetTypeOfLogging(new FileLogger(), false);
        task1.SetRepository(new FileSystemRepository(new PhysicalFileSystem()));
        task1.SetStorageAlgorithm(new SplitSaver());
        task1.AddObject(obj1);
        task1.AddObject(obj2);
        task1.AddPoint();
        BackupObject obj3 = new BackupObject(@"/bin/dash");
        task1.AddObject(obj3);
        task1.AddPoint();
        task1.SetRemovalPolicy(new AmountLimitAlgo(1));
        task1.SetPolicyForSystemRemoval(new FileSystemRemover());
        task1.ClearRestorePoints();
        task1.Serialize();
        Assert.Equal(1, task1.Backup.GetPoints().Count);
    }
}