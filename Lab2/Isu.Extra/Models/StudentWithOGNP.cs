using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Tools;
namespace Isu.Extra;

public class StudentWithOGNP : Student
{
    private Ognp? _chosenOgnp;
    private Megafaculty _faculty;
    private GroupInFaculty _mainGroup;
    private Stream? _firstGroup;
    private Stream? _secondGroup;

    public StudentWithOGNP(string name, GroupInFaculty group, Megafaculty faculty)
        : base(name, group)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new StudentException("Student's name cannot be null");
        if (group is null)
            throw new StudentException("Invalid group input");
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
        int flag1 = (from @group in ognp.Subjects[0].Stream from lesson1 in @group.Lessons from lesson2 in GroupInFaculty.Lessons where lesson1.Day == lesson2.Day && lesson1.Period == lesson2.Period select lesson1).Count();
        if (flag1 == ognp.Subjects[0].Stream.Count)
            throw new OgnpException("Student's schedule has intersections with OGNP schedule");

        int flag2 = (from @group in ognp.Subjects[1].Stream from lesson1 in @group.Lessons from lesson2 in GroupInFaculty.Lessons where lesson1.Day == lesson2.Day && lesson1.Period == lesson2.Period select lesson1).Count();
        if (flag2 == ognp.Subjects[1].Stream.Count)
        {
            throw new OgnpException("Student's schedule has intersections with OGNP schedule");
        }

        if (!ognp.Subjects[0].CheckAvailableSpaces())
            throw new StudentException("There is no available spaces in this OGNP");
        if (!ognp.Subjects[1].CheckAvailableSpaces())
            throw new StudentException("There is no available spaces in this OGNP");

        _chosenOgnp = ognp;

        foreach (var group in ognp.Subjects[0].Stream.Where(group => group.CheckFullness()))
        {
            _firstGroup = group.AddStudent(this);
            break;
        }

        foreach (var group in ognp.Subjects[1].Stream.Where(group => group.CheckFullness()))
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
}