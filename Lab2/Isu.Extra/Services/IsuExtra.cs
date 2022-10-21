using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Models;
using Isu.Tools;

namespace Isu.Extra.Services;

public class IsuExtra : IIsuExtra
{
    private List<StudentWithOgnp> _students;
    private List<Ognp> _registeredOgnp;
    private List<Megafaculty> _megafaculties;
    private List<GroupInFaculty> _groups;

    public IsuExtra()
    {
        _students = new List<StudentWithOgnp>();
        _registeredOgnp = new List<Ognp>();
        _megafaculties = new List<Megafaculty>();
        _groups = new List<GroupInFaculty>();
    }

    public Megafaculty AddMegafaculty(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new MegafacultyException("Invalid magefaculty name input");
        var megafaculty = new Megafaculty(name);
        if (megafaculty.AlreadyRegistered(_megafaculties))
        {
            throw new MegafacultyException("Such megafaculty is already registered");
        }

        _megafaculties.Add(megafaculty);
        return megafaculty;
    }

    public Ognp AddNewOgnp(string name, Megafaculty faculty, string firstSubject, string secondSubject, int amountOfStreams)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new OgnpException("OGNP name is incorrect");
        if (faculty is null)
            throw new OgnpException("There is no megafaculty for OGNP");
        if (string.IsNullOrWhiteSpace(firstSubject))
            throw new OgnpSubjectException("First stream name is incorrect");
        if (string.IsNullOrWhiteSpace(secondSubject))
            throw new OgnpSubjectException("Second stream name is incorrect");
        Ognp newOgnp = new Ognp(name, faculty);
        if (newOgnp.Exists(_registeredOgnp))
        {
            throw new OgnpException("Such OGNP is already registered");
        }

        _registeredOgnp.Add(newOgnp);
        OgnpSubject first = new OgnpSubject(firstSubject, newOgnp, amountOfStreams);
        OgnpSubject second = new OgnpSubject(secondSubject, newOgnp, amountOfStreams);
        newOgnp.AddSubjects(first, second);
        return newOgnp;
    }

    public GroupInFaculty AddGroupInFaculty(GroupName name, Megafaculty megafaculty)
    {
        var newGroup = new GroupInFaculty(name, megafaculty);
        if (newGroup.AlreadyRegistered(_groups))
        {
            throw new GroupException("Such group is already registered");
        }

        _groups.Add(newGroup);
        return newGroup;
    }

    public StudentWithOgnp AddStudent(string name, GroupInFaculty groupInFaculty, Megafaculty megafaculty)
    {
        var student = new StudentWithOgnp(name, groupInFaculty, megafaculty);
        if (student.AlreadyRegistered(_students))
        {
            throw new StudentException("Such student is already registered");
        }

        groupInFaculty.AddStudent(student);
        _students.Add(student);
        return student;
    }

    public Stream AddStreamToOgnp(string name, Ognp ognp, int numberOfSubject)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new OgnpException("Invalid stream name input");
        if (numberOfSubject < 1 || numberOfSubject > 2)
            throw new OgnpException("OGNP only has two subjects");
        var stream = new Stream(name, ognp.Subjects[numberOfSubject - 1], ognp);
        ognp.Subjects[numberOfSubject - 1].AddStreams(stream);
        return stream;
    }

    public void AddLessonToFacultyGroup(Lesson lesson, GroupInFaculty groupInFaculty)
    {
        if (lesson is null)
            throw new LessonException("Invalid lesson input");
        if (groupInFaculty is null)
            throw new GroupException("Invalid group input");
        groupInFaculty.AddLesson(lesson);
    }

    public void AddLessonToStream(Lesson lesson, Stream stream)
    {
        if (lesson is null)
            throw new LessonException("Invalid lesson input");
        if (stream is null)
            throw new OgnpException("Invalid stream input");
        stream.AddNewLesson(lesson);
    }

    public void ChooseOgnp(StudentWithOgnp student, Ognp ognp)
    {
        if (student.Ognp is null)
        {
            student.AddOgnpToStudent(ognp);
        }
        else
        {
            throw new StudentException("Student already has OGNP");
        }
    }

    public void DeleteEntry(StudentWithOgnp student)
    {
        if (student is null)
            throw new StudentException("Student input is incorrect");
        student.CancelOgnp();
    }

    public IReadOnlyList<Stream> GetGroupsOfFirstSubjectByOgnp(Ognp ognp)
    {
        return ognp.Subjects[0].Streams;
    }

    public IReadOnlyList<Stream> GetGroupsOfSecondSubjectByOgnp(Ognp ognp)
    {
        return ognp.Subjects[1].Streams;
    }

    public IEnumerable<StudentWithOgnp> GetStudentsWithRequiredOgnp(Ognp ognp)
    {
        if (ognp is null)
            throw new OgnpException("There is no ognp to get students");
        return ognp.Subjects[0].Streams.SelectMany(stream => stream.Students);
    }

    public IEnumerable<StudentWithOgnp> GetStudentsWithoutOgnp(Megafaculty faculty)
    {
        if (faculty is null)
            throw new MegafacultyException("Invalid megafaculty input");
        return _students.Where(student => student.Ognp == null && student.Megafaculty == faculty);
    }
}