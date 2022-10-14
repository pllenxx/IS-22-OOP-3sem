using System.Collections.ObjectModel;
using Isu.Models;
namespace Isu.Extra.Services;

public interface IIsuExtra
{
    Megafaculty AddMegafaculty(string name);

    Ognp AddNewOgnp(string name, Megafaculty faculty, string firstSubject, string secondSubject, int amountOfStreams);

    GroupInFaculty AddGroupInFaculty(GroupName name, Megafaculty megafaculty);

    StudentWithOGNP AddStudent(string name, GroupInFaculty groupInFaculty, Megafaculty megafaculty);

    Stream AddStreamToOgnp(string name, Ognp ognp, int numberOfSubject);

    void AddLessonToFacultyGroup(Lesson lesson, GroupInFaculty groupInFaculty);

    void AddLessonToStream(Lesson lesson, Stream stream);

    void ChooseOgnp(StudentWithOGNP student, Ognp ognp);

    void DeleteEntry(StudentWithOGNP student);

    (IReadOnlyList<Stream> list1, IReadOnlyList<Stream> list2) GetGroupsByOgnp(Ognp ognp);

    IEnumerable<StudentWithOGNP> GetStudentsWithRequiredOgnp(Ognp ognp);

    IEnumerable<StudentWithOGNP> GetStudentsWithoutOgnp(Megafaculty faculty);
}