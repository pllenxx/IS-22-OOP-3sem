using Isu.Models;
using Isu.Services;

namespace Isu;

internal class Program
{
    private IsuService _service = new IsuService();
    private void Main()
    {
        var group = _service.AddGroup(new GroupName("M32041"));
        var student = _service.AddStudent(group, "Kerim Eminov");
        var newGroup = group.AddStudent(student);
    }
}