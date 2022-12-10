using Zio;

namespace Backups.Extra.Remover;

public interface ISystemRemover
{
    void DeletePointsInSystem(IEnumerable<RestorePoint> points, BackupExtraTask task, ILogger logger, IFileSystem fs);
}