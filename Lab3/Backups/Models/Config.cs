using Backups.Services;
using Backups.Tools;

namespace Backups;

public class Config
{
    public Config(IBackup backup, IStorageAlgorithm algorithm, IRepository repository)
    {
        if (backup is null)
            throw new BackupException("Backup is null");
        if (algorithm is null)
            throw new BackupException("Algorithm is null");
        if (repository is null)
            throw new BackupException("Repository is null");
        Backup = backup;
        Algorithm = algorithm;
        Repository = repository;
    }

    public IBackup Backup { get; }
    public IStorageAlgorithm Algorithm { get; }
    public IRepository Repository { get; }
}