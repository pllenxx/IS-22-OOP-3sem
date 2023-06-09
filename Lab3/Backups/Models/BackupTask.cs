using Backups.Services;
using Backups.Tools;

namespace Backups;

public class BackupTask
{
    private IStorageAlgorithm _algorithm = null!;
    private IRepository _repository = null!;

    public BackupTask(string backupName, string path)
    {
        if (string.IsNullOrWhiteSpace(backupName))
            throw new BackupException("Incorrect backup task name");
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupException("Invalid backup path input");
        BackupName = backupName;
        BackupPath = path;
        Objects = new List<BackupObject>();
        Backup = new Backup();
    }

    public string BackupPath { get; private set; }
    public ICollection<BackupObject> Objects { get; private set; }
    public IBackup Backup { get; }
    public string BackupName { get; }
    public Config Configuration { get; private set; } = null!;

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

    public RestorePoint AddPoint()
    {
        IEnumerable<Storage> storages = _algorithm.Save(this);
        var restorePoint = new RestorePoint(DateTime.Now, storages);
        restorePoint.AddStorages(storages);
        foreach (var storage in storages)
        {
            storage.SetPoint(restorePoint);
        }

        Backup.AddRestorePoint(restorePoint);
        var directoryPath = Path.Combine(BackupPath, BackupName, restorePoint.Name);
        _repository.CreateDirectory(directoryPath, storages);
        var config = new Config(Backup, _algorithm, _repository);
        Configuration = config;
        return restorePoint;
    }

    public void AddObject(BackupObject backupObject)
    {
        if (backupObject is null)
            throw new BackupException("Backup object does not exist");
        Objects.Add(backupObject);
    }

    public void RemoveObject(BackupObject backupObject)
    {
        if (backupObject is null)
            throw new BackupException("Backup object is null");
        Objects.Remove(backupObject);
    }
}