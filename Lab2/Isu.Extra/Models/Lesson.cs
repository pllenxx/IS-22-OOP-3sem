using System.Reflection;
using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Tools;

namespace Isu.Extra;

public class Lesson
{
    private const int MinRoomNumber = 0;
    private Stream? _group;
    private GroupInFaculty? _groupInFaculty;
    private string _teacher;
    private int _room;

    public Lesson(Stream group, TimePeriod time, string teacher, int room, string day)
    {
        if (group is null)
            throw new GroupException("Invalid input of group");
        if (time is null)
            throw new TimeException("Invalid input of time");
        if (string.IsNullOrWhiteSpace(teacher))
            throw new TeacherException("Teacher's name cannot be null");
        if (room <= MinRoomNumber)
            throw new LessonException("Number of the room must be greater than 0");
        _group = group;
        _teacher = teacher;
        Period = time;
        _room = room;
        Day = day;
        _groupInFaculty = null;
    }

    public Lesson(GroupInFaculty group, TimePeriod time, string teacher, int room, string day)
    {
        if (group is null)
            throw new GroupException("Invalid input of group");
        if (time is null)
            throw new TimeException("Invalid input of time");
        if (string.IsNullOrWhiteSpace(teacher))
            throw new TeacherException("Teacher's name cannot be null");
        if (room <= MinRoomNumber)
            throw new LessonException("Number of the room must be greater than 0");
        _group = null;
        _teacher = teacher;
        Period = time;
        _room = room;
        Day = day;
        _groupInFaculty = group;
    }

    public TimePeriod Period { get; }
    public string Day { get; }

    protected internal bool CheckIntersections(IReadOnlyList<Lesson> lessons)
    {
        if (lessons is null)
            throw new LessonException("Invalid lesson list input");
        return lessons.Any(lesson => Day == lesson.Day && Period == lesson.Period);
    }
}