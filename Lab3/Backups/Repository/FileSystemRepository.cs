using System.IO.Compression;
using Backups.Services;
using Backups.Tools;
using Zio;
using Zio.FileSystems;

namespace Backups;

public class FileSystemRepository : IRepository
{
    public FileSystemRepository()
    {
    }

    public void CreateDirectory(string path, byte[] content)
    {
        if (path is null)
            throw new BackupException("Directory path is null");
        var fs = new PhysicalFileSystem();
        fs.CreateDirectory(path);
        fs.WriteAllBytes(path, content);
    }

    public void AddToRepository(string objectPath, string repositoryPath)
    {
        if (objectPath is null)
            throw new BackupException("Object path is null");
        if (repositoryPath is null)
            throw new BackupException("Directory path is null");
        var fs = new PhysicalFileSystem();
        byte[] content = fs.ReadAllBytes(objectPath);
        fs.WriteAllBytes(repositoryPath, content);
    }
}