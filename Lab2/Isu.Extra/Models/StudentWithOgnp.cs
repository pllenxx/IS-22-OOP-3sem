using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Tools;
namespace Isu.Extra;

public class StudentWithOgnp : Student
{
    private Ognp? _chosenOgnp;
    private Megafaculty _faculty;
    private GroupInFaculty _mainGroup;
    private Stream? _firstGroup;
    private Stream? _secondGroup;

    public StudentWithOgnp(string name, GroupInFaculty group, Megafaculty faculty)
        : base(name, group)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new StudentException("Student's name cannot be null");
        if (group is null)
            throw new StudentException("Invalid group input");
        if (faculty is null)
            throw new StudentException("Invalid faculty input");
        _mainGroup = group;
        _faculty = faculty;
        _chosenOgnp = null;
        _firstGroup = null;
        _secondGroup = null;
    }

    public GroupInFaculty GroupInFaculty => _mainGroup;
    public Megafaculty Megafaculty => _faculty;
    public Ognp? Ognp => _chosenOgnp ?? null;
    public Stream FirstStream => _firstGroup ?? throw new StudentException("First group og OGNP is not found");
    public Stream SecondStream => _secondGroup ?? throw new StudentException("Second group og OGNP is not found");

    protected internal void AddOgnpToStudent(Ognp ognp)
    {
        if (ognp is null)
            throw new OgnpException("Invalid OGNP input");
        if (ognp.Faculty == _faculty)
            throw new StudentException("Student cannot choose OGNP of his megafaculty");

        var amountOfStreamsWithIntersections1 = ognp.Subjects[0].Streams.SelectMany(stream => stream.Lessons).Count(lesson => lesson.CheckIntersections(GroupInFaculty.Lessons));
        if (amountOfStreamsWithIntersections1 == ognp.Subjects[0].Streams.Count)
        {
            throw new OgnpException("Student's schedule has intersections with OGNP schedule");
        }

        var amountOfStreamsWithIntersections2 = ognp.Subjects[1].Streams.SelectMany(stream => stream.Lessons).Count(lesson => lesson.CheckIntersections(GroupInFaculty.Lessons));
        if (amountOfStreamsWithIntersections2 == ognp.Subjects[1].Streams.Count)
        {
            throw new OgnpException("Student's schedule has intersections with OGNP schedule");
        }

        if (!ognp.Subjects[0].CheckAvailableSpaces())
            throw new StudentException("There is no available spaces in this OGNP");
        if (!ognp.Subjects[1].CheckAvailableSpaces())
            throw new StudentException("There is no available spaces in this OGNP");

        _chosenOgnp = ognp;

        var selectedGroupsFirstSubject = ognp.Subjects[0].Streams.Where(group => group.CheckFullness());
        foreach (var group in selectedGroupsFirstSubject)
        {
            _firstGroup = group.AddStudent(this);
            break;
        }

        var selectedGroupsSecondSubject = ognp.Subjects[1].Streams.Where(group => group.CheckFullness());
        foreach (var group in selectedGroupsSecondSubject)
        {
            _secondGroup = group.AddStudent(this);
            break;
        }
    }

    protected internal void CancelOgnp()
    {
        _chosenOgnp = null;
        _firstGroup?.RemoveStudent(this);
        _secondGroup?.RemoveStudent(this);
    }

    protected internal bool AlreadyRegistered(List<StudentWithOgnp> registeredStudents)
    {
        if (registeredStudents is null)
            throw new StudentException("Invalid student list input");
        return registeredStudents.Any(student => student == this);
    }
}