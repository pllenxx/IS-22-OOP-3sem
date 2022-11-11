using Backups.Services;
using Xunit;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void SplitStorageTest()
    {
        IStorageAlgorithm algorithm = new SplitSaver();
        FileSystemRepository repository = new FileSystemRepository();
        IBackup backup = new Backup();
        string generalPath = @"/Users/khartanovichp/Desktop/forbackup";
        BackupObject obj1 = new BackupObject(@"/Users/khartanovichp/Desktop/file1.txt");
        BackupObject obj2 = new BackupObject(@"/Users/khartanovichp/Desktop/file2.txt");
        IEnumerable<BackupObject> objects = new[] { obj1, obj2 };
        Config config = new Config(backup, algorithm, repository);
        BackupTask task = new BackupTask("taskOne", generalPath);
        task.SetRepository(repository);
        task.SetStorageAlgorithm(algorithm);
        task.AddPoint(objects);
    }
}