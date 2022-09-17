using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using Isu.Entities;
using Isu.Models;
using Isu.Services;
using Isu.Tools;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService _service = new IsuService();
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var group = _service.AddGroup(new GroupName("M32041"));
        var student = _service.AddStudent(group, "Kerim Eminov");
        Assert.Contains(student, group.GetStudents());
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var group = _service.AddGroup(new GroupName("M3100"));
        for (var i = 0; i < group.GetMaxAmount(); i++)
            _service.AddStudent(group, "Andrey Kozlov");
        Assert.Throws<GroupException>(() => _service.AddStudent(group, "Denis Zolotoy"));
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        Assert.Throws<GroupException>(() => _service.AddGroup(new GroupName("EEEEEEEEEEEE")));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var oldGroup = _service.AddGroup(new GroupName("M3111"));
        var newGroup = _service.AddGroup(new GroupName("M3101"));
        var student = _service.AddStudent(oldGroup, "Xenia Vasyutina");
        _service.ChangeStudentGroup(student, newGroup);
        Assert.Contains(student, newGroup.GetStudents());
    }
}