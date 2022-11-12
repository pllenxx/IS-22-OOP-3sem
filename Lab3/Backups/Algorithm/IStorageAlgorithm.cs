namespace Backups.Services;

public interface IStorageAlgorithm
{
    List<Storage> Save(BackupTask task);
}