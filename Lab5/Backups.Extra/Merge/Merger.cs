using Backups.Extra.Tools;

namespace Backups.Extra.Merge;

public class Merger
{
    public void Merge(IEnumerable<RestorePoint> points, BackupExtraTask task, ILogger logger)
    {
        if (!points.Any())
            throw new BackupsExtraException("Nothing to merge");
        if (task is null)
            throw new BackupsExtraException("Task is null");
        if (logger is null)
            throw new BackupsExtraException("Logger is null");
        var sortedPoints = new List<RestorePoint>(points.OrderByDescending(point => point.TimeOfCreation));
        Directory.CreateDirectory(Path.Combine(task.BackupPath, task.BackupName, "Merged_point"));
        var backupObjectsBytes = new List<byte[]>();
        backupObjectsBytes.Add(points.First().Storages.First().Bytes.First());
        foreach (var point in sortedPoints)
        {
            Directory.Delete(Path.Combine(task.BackupPath, task.BackupName, point.Name));
            task.Backup.DeleteRestorePoint(point);
            logger.Logging($"{point.Name} was deleted as doesn't fit the requirement");
        }

        var storages = sortedPoints
            .Where(point => !point.Storages.Count.Equals(1))
            .SelectMany(pt => pt.Storages)
            .SelectMany(storage => storage.Bytes)
            .Distinct()
            .Select(bytes =>
            {
                backupObjectsBytes.Add(bytes);
                List<byte[]> bt = new List<byte[]>(1) { bytes };
                Storage st = new Storage(Path.Combine(task.BackupPath, task.BackupName, "Merged_point", $"Archive№{Guid.NewGuid()}.zip"), bt);
                return st;
            })
            .ToList();

        foreach (var b in backupObjectsBytes)
        {
            File.WriteAllBytes(Path.Combine(task.BackupPath, task.BackupName, "Merged_point", $"Archive№{Guid.NewGuid()}.zip"), b);
        }

        RestorePoint resultPoint = new RestorePoint(DateTime.Now, storages);
        task.Backup.AddRestorePoint(resultPoint);
    }
}