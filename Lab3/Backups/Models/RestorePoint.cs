using Backups.Tools;

namespace Backups;

public class RestorePoint
{
    private static int _id = 1;
    private DateTime _timeOfCreation;
    private IEnumerable<Storage> _storages;

    public RestorePoint(string path, DateTime time)
    {
        if (string.IsNullOrEmpty(path))
            throw new BackupException("Path is null");
        _id = _id++;
        _timeOfCreation = time;
        Path = path;
        _storages = new List<Storage>();
    }

    public string Path { get; }

    public void AddStorages(IEnumerable<Storage> storages)
    {
        if (storages is null)
            throw new Exception("Storages do not exist");
        _storages = storages;
        foreach (var storage in storages)
        {
            storage.SetPoint(this);
        }
    }
}