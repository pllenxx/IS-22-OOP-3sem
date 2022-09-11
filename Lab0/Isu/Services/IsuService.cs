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
            throw new GroupException("Group with such name already exists!");
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
                           throw new StudentException("Not found!");
        return required;
    }

    public Student? FindStudent(int id)
    {
        foreach (Student student in _listOfStudents)
        {
            if (student.Id == id)
                return student;
        }

        return null;
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        foreach (Group group in _listOfGroups)
        {
            if (group.GetName() == groupName)
                return group.GetStudents();
        }

        throw new StudentException("Not found!");
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        List<Student> required = _listOfStudents.FindAll(student => student.Group.GetCourse() == courseNumber) ??
                               throw new GroupException("Not found!");
        return required;
    }

    public Group? FindGroup(GroupName groupName)
    {
        foreach (Group group in _listOfGroups)
        {
            if (group.GetName() == groupName)
                return group;
        }

        return null;
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        List<Group> required = _listOfGroups.FindAll(group => group.GetCourse() == courseNumber) ??
                               throw new GroupException("Not found!");
        return required;
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        Group oldGroup = student.Group;
        newGroup.AddStudent(student);
        oldGroup.RemoveStudent(student);
        student.Group = newGroup;
    }
}