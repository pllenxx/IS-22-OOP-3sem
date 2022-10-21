using Isu.Extra.Tools;

namespace Isu.Extra;

public class Megafaculty
{
    private int _id;
    private List<StudentWithOgnp> _students;
    private List<GroupInFaculty> _groups;

    public Megafaculty(string faculty)
    {
        if (string.IsNullOrWhiteSpace(faculty))
            throw new MegafacultyException("There is no such megafaculty in ITMO");
        try
        {
            _id = (int)Enum.Parse(typeof(Megafaculties), faculty);
        }
        catch (ArgumentException)
        {
            throw new MegafacultyException("There is no such megafaculty in ITMO");
        }

        _groups = new List<GroupInFaculty>();
        _students = new List<StudentWithOgnp>();
    }

    public IReadOnlyList<StudentWithOgnp> StudentWithOgnp => _students;

    protected internal bool AlreadyRegistered(List<Megafaculty> registeredMegafaculties)
    {
        if (registeredMegafaculties is null)
            throw new MegafacultyException("Invalid megafaculty list input");
        return registeredMegafaculties.Any(megafaculty => megafaculty == this);
    }
}
