using Isu.Extra.Tools;

namespace Isu.Extra;

public class OgnpSubject
{
    private const int MinAmountOfStreams = 1;
    private const int MaxAmountOfStreams = 5;
    private string _name;
    private List<Stream> _groups;
    private Ognp _ognp;

    public OgnpSubject(string name, Ognp ognp, int amountOfStreams)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new OgnpSubjectException("Invalid input of stream name");
        if (ognp is null)
            throw new OgnpException("Invalid OGNP input");
        if (amountOfStreams is < MinAmountOfStreams or > MaxAmountOfStreams)
            throw new OgnpException("Not enough streams");
        _name = name;
        _groups = new List<Stream>(amountOfStreams);
        _ognp = ognp;
    }

    public IReadOnlyList<Stream> Streams => _groups;

    protected internal void AddStreams(Stream stream)
    {
        if (stream is null)
            throw new OgnpSubjectException("Invalid stream input");
        if (_groups.Contains(stream))
            throw new OgnpSubjectException("This subjects already contains this stream");
        _groups.Add(stream);
    }

    protected internal bool CheckAvailableSpaces()
    {
        int availableSpaces = _groups.Count(group => group.CheckFullness());

        return availableSpaces == _groups.Count;
    }
}
