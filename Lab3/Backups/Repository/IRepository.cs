namespace Backups.Services;

public interface IRepository
{
    void CreateDirectory(string path, IEnumerable<Storage> storages);
}