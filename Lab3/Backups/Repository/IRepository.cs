using Zio;

namespace Backups.Services;

public interface IRepository
{
    void CreateDirectory(string path, byte[] content);
    void AddToRepository(string objectPath, string repositoryPath);
}