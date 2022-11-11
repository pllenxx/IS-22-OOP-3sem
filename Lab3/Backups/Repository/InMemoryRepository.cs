using Backups.Services;
using Backups.Tools;
using Zio;
using Zio.FileSystems;

namespace Backups;

public class InMemoryRepository : IRepository
{
    public InMemoryRepository()
    {
    }

    public void CreateDirectory(string path, byte[] content)
    {
        if (string.IsNullOrEmpty(path))
            throw new BackupException("Path cannot be empty");
        var fs = new MemoryFileSystem();
        fs.WriteAllBytes(path, content);
    }

    public void AddToRepository(string objectPath, string repositoryPath)
    {
        throw new NotImplementedException();
    }

    public void AddFileToRepository(string path, byte[] content)
    {
        throw new NotImplementedException();
    }
}