using System.ComponentModel.Design;
using Isu.Extra.Tools;

namespace Isu.Extra;

public class Ognp
{
    private string _name;
    private List<OgnpSubject> _subjects;

    public Ognp(string name, Megafaculty faculty)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new OgnpException("Invalid input of OGNP name");
        if (faculty is null)
            throw new MegafacultyException("Invalid megafaculty input");
        _name = name;
        _subjects = new List<OgnpSubject>(2);
        Faculty = faculty;
    }

    public IReadOnlyList<OgnpSubject> Subjects => _subjects;
    public Megafaculty Faculty { get; private set; }

    protected internal void AddSubjects(OgnpSubject first, OgnpSubject second)
    {
        if (first is null || second is null)
            throw new OgnpSubjectException("Invalid input of subjects");
        _subjects.Add(first);
        _subjects.Add(second);
    }

    protected internal bool Exists(List<Ognp> registeredOgnp)
    {
        if (registeredOgnp is null)
            throw new OgnpException("Invalid OGNP list input");
        return registeredOgnp.Any(ognp => ognp == this);
    }
}