using Isu.Extra.Tools;

namespace Isu.Extra;

public class Megafaculty
{
    private int _id;
    private List<StudentWithOGNP> _students;
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
        _students = new List<StudentWithOGNP>();
    }

    protected internal enum Megafaculties
    {
        KTU = 1,
        TINT,
        FT,
        BTINS,
        FTMI,
    }

    public IReadOnlyList<StudentWithOGNP> StudentWithOgnp => _students;
}
