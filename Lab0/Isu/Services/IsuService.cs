using System;
using Isu.Entities;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private List<Group> _listOfGroups = new List<Group>();
    private List<Student> _listOfStudents = new List<Student>();

    public Group AddGroup(GroupName name)
    {
        var group = new Group(name);
        if (_listOfGroups.Contains(group))
            throw new GroupException("Group with such name already exists");
        _listOfGroups.Add(group);
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        var student = new Student(name, group);
        group.AddStudent(student);
        _listOfStudents.Add(student);
        return student;
    }

    public Student GetStudent(int id)
    {
        Student required = _listOfStudents.Find(student => student.Id == id) ??
                           throw new StudentException("Not found");
        return required;
    }

    public Student? FindStudent(int id)
    {
        return _listOfStudents.FirstOrDefault(student => student.Id == id);
    }

    public IReadOnlyList<Student>? FindStudents(GroupName groupName)
    {
        return (from @group in _listOfGroups where @group.GetName() == groupName select @group.GetStudents())
            .FirstOrDefault();
    }

    public IReadOnlyList<Student>? FindStudents(CourseNumber courseNumber)
    {
        IReadOnlyList<Student> required = _listOfStudents.FindAll(student => student.Group.GetCourse() == courseNumber);
        return required ?? null;
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _listOfGroups.FirstOrDefault(group => group.GetName() == groupName);
    }

    public IReadOnlyList<Group>? FindGroups(CourseNumber courseNumber)
    {
        IReadOnlyList<Group> required = _listOfGroups.FindAll(group => group.GetCourse() == courseNumber);
        return required ?? null;
    }

    public void ChangeStudentGroup(Student? student, Group? newGroup)
    {
        if (student is null) return;
        if (newGroup is null) return;
        if (newGroup.GetName().GetCourse() != student.Group.GetName().GetCourse())
            throw new GroupException("This transfer is not possible");
        student.ChangeGroup(newGroup);
    }
}