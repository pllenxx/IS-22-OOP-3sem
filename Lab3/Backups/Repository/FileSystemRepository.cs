using System.IO.Compression;
using Backups.Services;
using Backups.Tools;
using Zio;
using Zio.FileSystems;

namespace Backups;

public class FileSystemRepository : IRepository
{
    private PhysicalFileSystem _fs;
    public FileSystemRepository(PhysicalFileSystem fs)
    {
        if (fs is null)
            throw new BackupException("Something is wrong with file system");
        _fs = fs;
    }

    public void CreateDirectory(string path, IEnumerable<Storage> storages)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupException("Invalid directory path input");
        if (!storages.Any())
            throw new BackupException("Unable to create a directory with zero storages");
        _fs.CreateDirectory(path);
        foreach (var storage in storages)
        {
            foreach (var byteArray in storage.Bytes)
            {
                 _fs.WriteAllBytes(Path.Combine(path, storage.Path), byteArray);
            }
        }
    }
}