using Backups.Extra.Tools;
using Serilog;

namespace Backups.Extra;

public class ConsoleLogger : ILogger
{
    private bool _timecodeOn;
    public void SetUpLogger(bool isTimeCodeNeeded)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
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