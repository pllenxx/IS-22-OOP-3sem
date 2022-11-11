using System.IO.Compression;
using Backups.Tools;

namespace Backups;

public class ZipArchiver
{
    public ZipArchiver()
    {
    }

    public byte[] GetContent(IEnumerable<Storage> storages)
    {
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, mode: ZipArchiveMode.Create, true))
        {
            foreach (var storage in storages)
            {
                // if (File.Exists(storage.Path))
                var zipArchiveEntry = archive.CreateEntry(storage.Path, CompressionLevel.Fastest);

                using var entryStream = zipArchiveEntry.Open();
                entryStream.Write(File.ReadAllBytes(storage.Path), 0, File.ReadAllBytes(storage.Path).Length);

                // else if (Directory.Exists(storage.Path))
                // {
                //     foreach (var file in Directory.GetFiles(storage.Path))
                //     {
                //         var entryName = Path.GetFileName(file);
                //         var entry = archive.CreateEntry(entryName);
                //         entry.LastWriteTime = File.GetLastWriteTime(file);
                //         using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                //         using (var stream = entry.Open())
                //         {
                //             fs.CopyTo(stream);
                //         }
                //     }
                // }
                // else
                // {
                //     throw new BackupException("Undefined type");
                // }
            }
        }

        var archiveFile = memoryStream.ToArray();
        return archiveFile;
    }
}