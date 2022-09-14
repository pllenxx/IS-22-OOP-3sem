using System;
using Isu.Tools;
namespace Isu.Models;

public class GroupName
{
    private const int _minNameLength = 5;
    private const int _maxNameLength = 6;
    private string _name;

    public GroupName(string? name)
    {
        if (string.IsNullOrEmpty(name))
            throw new GroupException("Group name does not exist");
        if (name[0] != 'M' || name[1] != '3')
            throw new GroupException("Group does not belong to Information Systems faculty");
        if (name.Length < _minNameLength || name.Length > _maxNameLength)
            throw new GroupException("Invalid input");
        _name = name;
    }

    public int GetCourse() => (int)char.GetNumericValue(_name[2]);
}