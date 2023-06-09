using System;
using System.Diagnostics;
using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;
public class Group
{
    private const int _maxAmountOfStudentsInGroup = 25;
    private GroupName _name;
    private CourseNumber _course;
    private List<Student> _listOfStudents;

    public Group(GroupName name)
    {
        _name = name ?? throw new GroupException("Name of this does not exist");
        _course = new CourseNumber(name.GetCourse());
        _listOfStudents = new List<Student>();
    }

    public Group AddStudent(Student newStudent)
    {
        if (newStudent is null)
            throw new StudentException("There is no one to add to the group");
        if (_listOfStudents.Count == _maxAmountOfStudentsInGroup)
            throw new GroupException("Number of students per group exceeded");
        _listOfStudents.Add(newStudent);
        return this;
    }

    public Group RemoveStudent(Student student)
    {
        if (student is null)
            throw new StudentException("There is no one to remove from the group");
        _listOfStudents.Remove(student);
        return this;
    }

    public int GetMaxAmount() => _maxAmountOfStudentsInGroup;
    public GroupName GetName() => _name;

    public IReadOnlyList<Student> GetStudents() => _listOfStudents;

    public CourseNumber GetCourse() => _course;
}