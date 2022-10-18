using Isu.Extra.Tools;

namespace Isu.Extra;

public class TimePeriod
{
    private DateTime _start;
    private DateTime _end;

    public TimePeriod(string start, string end)
    {
        if (!DateTime.TryParse(start, out _start))
            throw new TimeException("Start of the lesson is incorrect");
        if (!DateTime.TryParse(end, out _end))
            throw new TimeException("End of the lesson is incorrect");

        if (_end - _start != TimeSpan.Parse("1:30:00"))
            throw new TimeException("Lesson must be 90 minutes long");
    }
}