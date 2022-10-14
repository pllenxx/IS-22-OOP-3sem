using Isu.Extra.Tools;

namespace Isu.Extra;

public class OgnpSubject
{
    private string _name;
    private List<Stream> _groups;
    private Ognp _ognp;

    public OgnpSubject(string name, Ognp ognp, int amountOfStreams)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new OgnpSubjectException("Invalid input of stream name");
        _name = name;
        _groups = new List<Stream>(amountOfStreams);
        _ognp = ognp;
    }

    public List<Stream> Stream => _groups;

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
        int flag = _groups.Count(group => group.CheckFullness());

        return flag == _groups.Count;
    }
}
