using Backups.Extra.Merge;
using Backups.Extra.Recover;
using Backups.Extra.Remover;
using Backups.Extra.Tools;
using Newtonsoft.Json;
using Zio;

namespace Backups.Extra;

public class BackupExtraTask : BackupTask
{
   private ILimitAlgorithm _limitAlgorithm = null!;
   private ISystemRemover _systemRemover = null!;
   private IRestore _restore = null!;
   private ILogger _logger = null!;
   private Merger _merger;
   private IFileSystem _fileSystem = null!;
   public BackupExtraTask(string name, string path)
      : base(name, path)
   {
      _merger = new Merger();
   }

   public void SetRemovalPolicy(ILimitAlgorithm remover)
   {
      if (remover is null)
         throw new BackupsExtraException("Remover policy is null");
      _limitAlgorithm = remover;
      _logger.Logging("Removal algorithm is set");
   }

   public void SetPolicyForSystemRemoval(ISystemRemover remover, IFileSystem fileSystem)
   {
      if (remover is null)
         throw new BackupsExtraException("Remover policy is null");
      if (fileSystem is null)
         throw new BackupsExtraException("File system is null");
      _systemRemover = remover;
      _fileSystem = fileSystem;
      _logger.Logging("Removing from system type is set");
   }

   public void SetRecoverPolicy(IRestore restore)
   {
      if (restore is null)
         throw new BackupsExtraException("Recover policy is null");
      _restore = restore;
      _logger.Logging("Restoring type is set");
   }

   public void SetTypeOfLogging(ILogger logger, bool timeCodeOn)
   {
      if (logger is null)
         throw new BackupsExtraException("Logger is null");
      _logger = logger;
      _logger.SetUpLogger(timeCodeOn);
   }

   public void ClearRestorePoints()
   {
      var pointsToDelete = _limitAlgorithm.FindPoints(Backup.GetPoints());
      _systemRemover.DeletePointsInSystem(pointsToDelete, this, _logger, _fileSystem);
   }

   public void MergeRestorePoints()
   {
      var pointsToMerge = _limitAlgorithm.FindPoints(Backup.GetPoints());
      _merger.Merge(pointsToMerge, this, _logger);
   }

   public void RestoreBackupObjects()
   {
      _restore.RestoreObjects(this, _logger);
   }

   public void Serialize()
   {
      var json = JsonConvert.SerializeObject(this, Formatting.Indented);
      File.WriteAllText(Path.Combine(BackupPath, BackupName, "InfoJson.txt"), json);
      _logger.Logging("Json with backup info was created");
   }
}