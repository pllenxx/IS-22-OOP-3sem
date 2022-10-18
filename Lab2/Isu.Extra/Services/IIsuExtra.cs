using System.Collections.ObjectModel;
using Isu.Models;
namespace Isu.Extra.Services;

public interface IIsuExtra
{
    Megafaculty AddMegafaculty(string name);

    Ognp AddNewOgnp(string name, Megafaculty faculty, string firstSubject, string secondSubject, int amountOfStreams);

    GroupInFaculty AddGroupInFaculty(GroupName name, Megafaculty megafaculty);

    StudentWithOgnp AddStudent(string name, GroupInFaculty groupInFaculty, Megafaculty megafaculty);

    Stream AddStreamToOgnp(string name, Ognp ognp, int numberOfSubject);

    void AddLessonToFacultyGroup(Lesson lesson, GroupInFaculty groupInFaculty);

    void AddLessonToStream(Lesson lesson, Stream stream);

    void ChooseOgnp(StudentWithOgnp student, Ognp ognp);

    void DeleteEntry(StudentWithOgnp student);

    IReadOnlyList<Stream> GetGroupsOfFirstSubjectByOgnp(Ognp ognp);

    IReadOnlyList<Stream> GetGroupsOfSecondSubjectByOgnp(Ognp ognp);

    IEnumerable<StudentWithOgnp> GetStudentsWithRequiredOgnp(Ognp ognp);

    IEnumerable<StudentWithOgnp> GetStudentsWithoutOgnp(Megafaculty faculty);
}