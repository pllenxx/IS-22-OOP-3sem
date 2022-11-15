using System.IO.Compression;
using Backups.Tools;

namespace Backups;

public class ZipArchiver
{
    public ZipArchiver()
    {
    }

    public byte[] GetContent(IReadOnlyList<BackupObject> objects)
    {
        if (!objects.Any())
            throw new BackupException("Nothing to archive");
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, mode: ZipArchiveMode.Create, true))
        {
            foreach (var backupObject in objects)
            {
                if (File.Exists(backupObject.Path))
                {
                    var zipArchiveEntry = archive.CreateEntry(backupObject.Path, CompressionLevel.Fastest);

                    using var entryStream = zipArchiveEntry.Open();
                    entryStream.Write(File.ReadAllBytes(backupObject.Path), 0, File.ReadAllBytes(backupObject.Path).Length);
                }
                else if (Directory.Exists(backupObject.Path))
                {
                    foreach (var file in Directory.GetFiles(backupObject.Path))
                    {
                        var entryName = Path.GetFileName(file);
                        var entry = archive.CreateEntry(entryName);
                        entry.LastWriteTime = File.GetLastWriteTime(file);
                        using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (var stream = entry.Open())
                        {
                            fs.CopyTo(stream);
                        }
                    }
                }
                else
                {
                    throw new BackupException("Undefined type");
                }
            }
        }

        var archiveFile = memoryStream.ToArray();
        return archiveFile;
    }
}