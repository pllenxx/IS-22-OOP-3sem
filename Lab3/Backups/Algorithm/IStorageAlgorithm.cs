namespace Backups.Services;

public interface IStorageAlgorithm
{
    IEnumerable<Storage> Save(BackupTask task);
}