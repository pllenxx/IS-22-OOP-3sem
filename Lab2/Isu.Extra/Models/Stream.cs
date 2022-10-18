using System.ComponentModel;
using System.Security;
using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Models;
using Isu.Tools;

namespace Isu.Extra;

public class Stream
{
    private const int MaxAmountOfStudents = 30;
    private string _name;
    private OgnpSubject _subject;
    private Ognp _ognp;
    private List<StudentWithOgnp> _students;
    private List<Lesson> _schedule;
    private int _amountOfStudents;

    public Stream(string name, OgnpSubject subject, Ognp ognp)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new GroupException("Invalid input of group name");
        if (subject is null)
            throw new OgnpException("Invalid subject input");
        if (ognp is null)
            throw new OgnpException("Invalid ognp input");
        _name = name;
        _students = new List<StudentWithOgnp>();
        _schedule = new List<Lesson>();
        _amountOfStudents = 0;
        _subject = subject;
        _ognp = ognp;
    }

    public IReadOnlyList<StudentWithOgnp> Students => _students;
    public IReadOnlyList<Lesson> Lessons => _schedule;

    protected internal void AddNewLesson(Lesson lesson)
    {
        if (lesson is null)
            throw new LessonException("Invalid lesson input");
        if (_schedule.Contains(lesson))
            throw new OgnpException("This stream already has this lesson");
        if (lesson.CheckIntersections(Lessons))
        {
            throw new LessonException("This lesson intersects with the lesson in the stream");
        }

        _schedule.Add(lesson);
    }

    protected internal bool CheckFullness()
    {
        return _amountOfStudents < MaxAmountOfStudents;
    }

    protected internal Stream AddStudent(StudentWithOgnp student)
    {
        if (student is null)
            throw new StudentException("No student to add to the stream");
        if (_amountOfStudents++ < MaxAmountOfStudents)
        {
            _students.Add(student);
        }

        return this;
    }

    protected internal void RemoveStudent(StudentWithOgnp student)
    {
        if (student is null)
            throw new StudentException("Invalid student input");
        if (!_students.Contains(student))
            throw new StudentException("This student does not attend such OGNP");
        _students.Remove(student);
        _amountOfStudents--;
    }
}