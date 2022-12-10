using Backups.Extra.Tools;
using Zio;
using Zio.FileSystems;

namespace Backups.Extra.Remover;

public class InMemoryRemover : ISystemRemover
{
    public void DeletePointsInSystem(IEnumerable<RestorePoint> points, BackupExtraTask task, ILogger logger)
    {
        if (!points.Any())
            throw new BackupsExtraException("Amount of restore point to delete is zero");
        if (task is null)
            throw new BackupsExtraException("Backup task is null");
        if (logger is null)
            throw new BackupsExtraException("Logger is null");
        var fs = new MemoryFileSystem();
        foreach (var point in points)
        {
            if (fs.DirectoryExists(Path.Combine(task.BackupPath, task.BackupName, point.Name)))
            {
                fs.DeleteDirectory(Path.Combine(task.BackupPath, task.BackupName, point.Name), false);
                task.Backup.DeleteRestorePoint(point);
                logger.Logging($"{point.Name} was removed from memory");
            }
            else
            {
                logger.Logging($"Directory for {point.Name} was nor found");
                throw new BackupsExtraException("Restore point directory does not exist");
            }
        }
    }
}