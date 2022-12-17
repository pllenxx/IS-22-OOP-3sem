using Backups.Tools;

namespace Backups;

public class RestorePoint
{
    private IEnumerable<Storage> _storages;

    public RestorePoint(DateTime time, IEnumerable<Storage> storages)
    {
        if (!storages.Any())
            throw new BackupException("No storages to create a restore point");
        TimeOfCreation = time;
        Name = $"RestorePoint№{Guid.NewGuid()}";
        _storages = storages;
    }

    public IReadOnlyList<Storage> Storages => _storages.ToList().AsReadOnly();
    public string Name { get; }
    public DateTime TimeOfCreation { get;  }

    public void AddStorages(IEnumerable<Storage> storages)
    {
        if (!storages.Any())
            throw new BackupException("Nothing to add to restore point");
        _storages = storages;
        foreach (var storage in storages)
        {
            storage.SetPoint(this);
        }
    }
}