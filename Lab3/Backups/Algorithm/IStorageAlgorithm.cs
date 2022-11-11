namespace Backups.Services;

public interface IStorageAlgorithm
{
    IEnumerable<Storage> Save(string path, IEnumerable<BackupObject> objects);
}