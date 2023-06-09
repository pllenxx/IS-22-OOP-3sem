using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Models;
using Isu.Tools;

namespace Isu.Extra;

public class GroupInFaculty : Group
{
    private List<Lesson> _schedule;
    private List<StudentWithOgnp> _students;
    private Megafaculty _megafaculty;

    public GroupInFaculty(GroupName name, Megafaculty megafaculty)
        : base(name)
    {
        if (name is null)
            throw new GroupException("Invalid group name input");
        _schedule = new List<Lesson>();
        _students = new List<StudentWithOgnp>();
        _megafaculty = megafaculty;
    }

    public IReadOnlyList<Lesson> Lessons => _schedule;

    protected internal void AddLesson(Lesson lesson)
    {
        if (lesson is null)
            throw new LessonException("Invalid lesson name input");
        if (_schedule.Contains(lesson))
            throw new GroupException("This group already has this lesson");
        if (lesson.CheckIntersections(Lessons))
        {
            throw new GroupException("This lesson intersects with other lessons");
        }

        _schedule.Add(lesson);
    }

    protected internal bool AlreadyRegistered(List<GroupInFaculty> registeredGroups)
    {
        if (registeredGroups is null)
            throw new GroupException("Invalid group list input");
        return registeredGroups.Any(group => group == this);
    }
}
