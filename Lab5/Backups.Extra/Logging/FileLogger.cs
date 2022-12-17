using Backups.Extra.Tools;
using Serilog;

namespace Backups.Extra;

public class FileLogger : ILogger
{
    private bool _timecodeOn;
    public void SetUpLogger(bool isTimeCodeNeeded)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(@"/Users/khartanovichp/Desktop/forbackup/BackupsExtraLog.txt")
            .CreateLogger();
        _timecodeOn = isTimeCodeNeeded;
    }

    public void Logging(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new BackupsExtraException("Invalid log massage");
        Log.Information(_timecodeOn ? $"{DateTime.Now} message" : message);
    }
}