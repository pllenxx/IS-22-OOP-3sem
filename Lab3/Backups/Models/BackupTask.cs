using System.Security.AccessControl;
using Backups.Services;
using Backups.Tools;

namespace Backups;

public class BackupTask
{
    private string _name;
    private ICollection<BackupObject> _objects;
    private string _backupPath;
    private IBackup _backup;
    private IStorageAlgorithm _algorithm = null!;
    private IRepository _repository = null!;

    public BackupTask(string name, string backupPath)
    {
        if (string.IsNullOrEmpty(name))
            throw new BackupException("Incorrect backup task name");
        if (string.IsNullOrEmpty(backupPath))
            throw new BackupException("Incorrect backup task path");
        _name = name;
        _backupPath = backupPath;
        _objects = new List<BackupObject>();
        _backup = new Backup();
    }

    public void SetStorageAlgorithm(IStorageAlgorithm algorithm)
    {
        if (algorithm is null)
            throw new BackupException("Something is wrong with the algorithm. Try again");
        _algorithm = algorithm;
    }

    public void SetRepository(IRepository repository)
    {
        if (repository is null)
            throw new BackupException("Something is wrong with the repository. Try again");
        _repository = repository;
    }

    public RestorePoint AddPoint(IEnumerable<BackupObject> objects)
    {
        if (objects is null)
            throw new BackupException("Nothing to backup");
        _objects = (ICollection<BackupObject>)objects;
        var id = Guid.NewGuid();
        DirectoryInfo dir = Directory.CreateDirectory(_backupPath);
        var restorePointPath = Path.Combine(_backupPath, $"RestorePoint{id}");
        var restorePoint = new RestorePoint(restorePointPath, DateTime.Now);
        IEnumerable<Storage> storages = _algorithm.Save(restorePointPath, objects);
        restorePoint.AddStorages(storages);
        var archiver = new ZipArchiver();
        byte[] content = archiver.GetContent(storages);
        _repository.CreateDirectory(restorePointPath, content);
        return restorePoint;
    }

    public IEnumerable<BackupObject> RemoveObject(BackupObject backupObject)
    {
        if (backupObject is null)
            throw new BackupException("Backup object is null");
        _objects.Remove(backupObject);
        return _objects;
    }
}