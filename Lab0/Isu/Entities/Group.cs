using System;
using System.Diagnostics;
using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;
public class Group
{
    private const int _maxAmountOfStudentsInGroup = 25;
    private GroupName _nameOfTheGroup;
    private CourseNumber _course;
    private List<Student> _listOfStudentsInOneGroup;
    private int _amountOfStudents;

    public Group(GroupName name)
    {
        _nameOfTheGroup = name;
        _amountOfStudents = 0;
        _course = new CourseNumber(name.GetCourse());
        _listOfStudentsInOneGroup = new List<Student>();
    }

    public Group(GroupName name, int numberOfStudents)
    {
        if (numberOfStudents > _maxAmountOfStudentsInGroup)
            throw new GroupException("This number of students is not allowed in one group!");
        _nameOfTheGroup = name;
        _amountOfStudents = numberOfStudents;
        _course = new CourseNumber(name.GetCourse());
        _listOfStudentsInOneGroup = new List<Student>();
    }

    public Group AddStudent(Student newStudent)
    {
        if (++_amountOfStudents > _maxAmountOfStudentsInGroup)
            throw new GroupException("This number of students is not allowed in one group!");
        _listOfStudentsInOneGroup.Add(newStudent);
        return this;
    }

    public Group RemoveStudent(Student student)
    {
        _listOfStudentsInOneGroup.Remove(student);
        _amountOfStudents--;
        return this;
    }

    public int GetMaxAmount()
    {
        return _maxAmountOfStudentsInGroup;
    }

    public GroupName GetName()
    {
        return _nameOfTheGroup;
    }

    public List<Student> GetStudents()
    {
        return _listOfStudentsInOneGroup;
    }

    public CourseNumber GetCourse()
    {
        return _course;
    }
}