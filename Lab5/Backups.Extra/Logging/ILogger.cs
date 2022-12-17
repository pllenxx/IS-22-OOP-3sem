namespace Backups.Extra;

public interface ILogger
{
    void SetUpLogger(bool isTimeCodeNeeded);
    void Logging(string message);
}