using System;
using Isu.Tools;
namespace Isu.Models;

public class GroupName
{
    private string _name;

    public GroupName(string name)
    {
        if (name[0] != 'M' || name[1] != '3')
            throw new GroupException("Group does not belong to Information Systems faculty!");
        if (name.Length < 5 || name.Length > 6)
            throw new GroupException("Invalid input!");
        _name = name;
    }

    public int GetCourse()
    {
        return _name[2] - '0';
    }
}