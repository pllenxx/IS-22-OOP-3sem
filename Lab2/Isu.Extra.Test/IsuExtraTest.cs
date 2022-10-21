using Isu.Extra.Services;
using Isu.Extra.Tools;
using Isu.Models;
using Isu.Tools;
using Xunit;
namespace Isu.Extra.Test;

public class IsuExtraTest
{
    private IsuExtra _isuExtra = new IsuExtra();

    [Fact]
    public void AddLessonsToOgnp_OgnpHasLessons()
    {
        var megafaculty = _isuExtra.AddMegafaculty("BTINS");
        var ognp = _isuExtra.AddNewOgnp("ОГНП БТиНСа", megafaculty, "Неустойчивое развитие", "Правильно кушаем", 5);
        var firstStream = _isuExtra.AddStreamToOgnp("1.1", ognp, 1);
        var secondStream = _isuExtra.AddStreamToOgnp("1.2", ognp, 2);
        var firstLesson1 = new Lesson(firstStream, new TimePeriod("11:40 AM", "1:10 PM"), "Усталый Артем", 228, "Понедельник");
        var secondLesson1 = new Lesson(firstStream, new TimePeriod("1:30 PM", "3:00 PM"), "Усталый Артем", 228, "Понедельник");
        var firstLesson2 = new Lesson(secondStream, new TimePeriod("8:20 AM", "9:50 AM"), "Ослиный Андрей", 993, "Суббота");
        var secondLesson2 = new Lesson(secondStream, new TimePeriod("10:00 AM", "11:30 AM"), "Ослиный Андрей", 993, "Суббота");
        _isuExtra.AddLessonToStream(firstLesson1, firstStream);
        _isuExtra.AddLessonToStream(secondLesson1, firstStream);
        _isuExtra.AddLessonToStream(firstLesson2, secondStream);
        _isuExtra.AddLessonToStream(secondLesson2, secondStream);
        Assert.Contains(firstLesson1, firstStream.Lessons);
        Assert.Contains(firstLesson2, secondStream.Lessons);
    }

    [Fact]
    public void ChooseOgnp_OgnpHasStudent()
    {
        var firstMegafaculty = _isuExtra.AddMegafaculty("TINT");
        var group = _isuExtra.AddGroupInFaculty(new GroupName("M32091"), firstMegafaculty);
        var student = _isuExtra.AddStudent("Vika", group, firstMegafaculty);
        var secondMegafaculty = _isuExtra.AddMegafaculty("BTINS");
        var ognp = _isuExtra.AddNewOgnp("ОГНП БТиНСа", secondMegafaculty, "Неустойчивое развитие", "Правильно кушаем", 5);
        var firstStream = _isuExtra.AddStreamToOgnp("1.1", ognp, 1);
        var secondStream = _isuExtra.AddStreamToOgnp("1.2", ognp, 2);
        _isuExtra.ChooseOgnp(student, ognp);
        Assert.Equal(ognp, student.Ognp);
        Assert.Contains(student, firstStream.Students);
        Assert.Contains(student, secondStream.Students);
    }

    [Fact]
    public void StudentCannotChooseOgnpFromHisMegafaculty()
    {
        var megafaculty = _isuExtra.AddMegafaculty("TINT");
        var group = _isuExtra.AddGroupInFaculty(new GroupName("M32091"), megafaculty);
        var student = _isuExtra.AddStudent("Elf dar", group, megafaculty);
        var ognp = _isuExtra.AddNewOgnp("ОГНП ТИнТА", megafaculty, "Много прогаем", "Решаем математику", 3);
        Assert.Throws<StudentException>(() => _isuExtra.ChooseOgnp(student, ognp));
    }

    [Fact]
    public void CancelOgnp_StudentCanChooseAnotherOne()
    {
        var firstMegafaculty = _isuExtra.AddMegafaculty("TINT");
        var group = _isuExtra.AddGroupInFaculty(new GroupName("M32081"), firstMegafaculty);
        var student = _isuExtra.AddStudent("Su ren", group, firstMegafaculty);
        var secondMegafaculty = _isuExtra.AddMegafaculty("KTU");
        var ognp = _isuExtra.AddNewOgnp("ОГНП КТУ", secondMegafaculty, "Строим роботов", "Проектируем роботов", 5);
        var firstStream = _isuExtra.AddStreamToOgnp("2.1", ognp, 1);
        var secondStream = _isuExtra.AddStreamToOgnp("2.2", ognp, 2);
        var firstLesson1 = new Lesson(firstStream, new TimePeriod("11:40 AM", "1:10 PM"), "Суирный Миша", 1337, "Среда");
        var secondLesson1 = new Lesson(firstStream, new TimePeriod("1:30 PM", "3:00 PM"), "Суирный Миша", 1337, "Среда");
        var firstLesson2 = new Lesson(secondStream, new TimePeriod("8:20 AM", "9:50 AM"), "Занятой Сережа", 98, "Четверг");
        var secondLesson2 = new Lesson(secondStream, new TimePeriod("10:00 AM", "11:30 AM"), "Занятой Сережа", 98, "Четверг");
        _isuExtra.AddLessonToStream(firstLesson1, firstStream);
        _isuExtra.AddLessonToStream(secondLesson1, firstStream);
        _isuExtra.AddLessonToStream(firstLesson2, secondStream);
        _isuExtra.AddLessonToStream(secondLesson2, secondStream);
        _isuExtra.ChooseOgnp(student, ognp);
        Assert.Equal(ognp, student.Ognp);
        Assert.Contains(student, firstStream.Students);
        Assert.Contains(student, secondStream.Students);
        _isuExtra.DeleteEntry(student);
        Assert.NotEqual(student.Ognp, ognp);
        Assert.DoesNotContain(student, firstStream.Students);
        Assert.DoesNotContain(student, secondStream.Students);
    }

    [Fact]
    public void StudentCannotChooseOgnpWithIntersections()
    {
        var firstMegafaculty = _isuExtra.AddMegafaculty("TINT");
        var group = _isuExtra.AddGroupInFaculty(new GroupName("M32041"), firstMegafaculty);
        var student = _isuExtra.AddStudent("Polina", group, firstMegafaculty);
        var mainLesson = new Lesson(group, new TimePeriod("10:00 AM", "11:30 AM"), "Маятин Александр", 123, "Вторник");
        _isuExtra.AddLessonToFacultyGroup(mainLesson, group);
        var secondMegafaculty = _isuExtra.AddMegafaculty("FTMI");
        var ognp = _isuExtra.AddNewOgnp("ОГНП ФТМИ", secondMegafaculty, "Какой-то маркетинг", "Еще один маркетинг", 2);
        var someStream = _isuExtra.AddStreamToOgnp("3.1", ognp, 1);
        var firstLesson1 = new Lesson(someStream, new TimePeriod("10:00 AM", "11:30 AM"), "Энергичная Марина", 228, "Вторник");
        var secondLesson1 = new Lesson(someStream, new TimePeriod("11:40 AM", "1:10 PM"), "Энергичная Марина", 228, "Вторник");
        _isuExtra.AddLessonToStream(firstLesson1, someStream);
        _isuExtra.AddLessonToStream(secondLesson1, someStream);
        Assert.Throws<OgnpException>(() => _isuExtra.ChooseOgnp(student, ognp));
    }

    [Fact]
    public void CreateMegafacultyWithInvalidName()
    {
        Assert.Throws<MegafacultyException>(() => _isuExtra.AddMegafaculty("FITIP"));
    }

    [Fact]
    public void GetStudentsWithSpecialOgnp()
    {
        var firstMegafaculty = _isuExtra.AddMegafaculty("TINT");
        var group = _isuExtra.AddGroupInFaculty(new GroupName("M33001"), firstMegafaculty);
        var secondMegafaculty = _isuExtra.AddMegafaculty("FT");
        var ognp = _isuExtra.AddNewOgnp("ОГНП ФТ МФ", secondMegafaculty, "Что-то физическое", "Что-то физическое, но другое", 1);
        var student1 = _isuExtra.AddStudent("Ulya", group, firstMegafaculty);
        var student2 = _isuExtra.AddStudent("Meneme", group, firstMegafaculty);
        var firstStream = _isuExtra.AddStreamToOgnp("4.1", ognp, 1);
        var secondStream = _isuExtra.AddStreamToOgnp("4.2", ognp, 2);
        var firstLesson1 = new Lesson(firstStream, new TimePeriod("11:40 AM", "1:10 PM"), "Зинчик", 666, "Понедельник");
        var secondLesson1 = new Lesson(firstStream, new TimePeriod("1:30 PM", "3:00 PM"), "Зинчик", 666, "Понедельник");
        var firstLesson2 = new Lesson(secondStream, new TimePeriod("8:20 AM", "9:50 AM"), "Наира", 100, "Пятница");
        var secondLesson2 = new Lesson(secondStream, new TimePeriod("10:00 AM", "11:30 AM"), "Наира", 100, "Пятница");
        _isuExtra.AddLessonToStream(firstLesson1, firstStream);
        _isuExtra.AddLessonToStream(secondLesson1, firstStream);
        _isuExtra.AddLessonToStream(firstLesson2, secondStream);
        _isuExtra.AddLessonToStream(secondLesson2, secondStream);
        _isuExtra.ChooseOgnp(student1, ognp);
        _isuExtra.ChooseOgnp(student2, ognp);
        var required = _isuExtra.GetStudentsWithRequiredOgnp(ognp);
        Assert.Contains(student1, required);
        Assert.Contains(student2, required);
    }

    [Fact]
    public void GetStudentsWhoDoesntHaveOgnp()
    {
        var firstMegafaculty = _isuExtra.AddMegafaculty("TINT");
        var group = _isuExtra.AddGroupInFaculty(new GroupName("M33001"), firstMegafaculty);
        var secondMegafaculty = _isuExtra.AddMegafaculty("FT");
        var ognp = _isuExtra.AddNewOgnp("ОГНП ФТ МФ", secondMegafaculty, "Что-то физическое", "Что-то физическое, но другое", 1);
        var student1 = _isuExtra.AddStudent("Ulya", group, firstMegafaculty);
        var student2 = _isuExtra.AddStudent("Pasha", group, firstMegafaculty);
        var firstStream = _isuExtra.AddStreamToOgnp("4.1", ognp, 1);
        var secondStream = _isuExtra.AddStreamToOgnp("4.2", ognp, 2);
        var firstLesson1 = new Lesson(firstStream, new TimePeriod("11:40 AM", "1:10 PM"), "Зинчик", 666, "Понедельник");
        var secondLesson1 = new Lesson(firstStream, new TimePeriod("1:30 PM", "3:00 PM"), "Зинчик", 666, "Понедельник");
        var firstLesson2 = new Lesson(secondStream, new TimePeriod("8:20 AM", "9:50 AM"), "Наира", 100, "Пятница");
        var secondLesson2 = new Lesson(secondStream, new TimePeriod("10:00 AM", "11:30 AM"), "Наира", 100, "Пятница");
        _isuExtra.AddLessonToStream(firstLesson1, firstStream);
        _isuExtra.AddLessonToStream(secondLesson1, firstStream);
        _isuExtra.AddLessonToStream(firstLesson2, secondStream);
        _isuExtra.AddLessonToStream(secondLesson2, secondStream);
        _isuExtra.ChooseOgnp(student1, ognp);
        var required = _isuExtra.GetStudentsWithoutOgnp(firstMegafaculty);
        Assert.Contains(student2, required);
        Assert.DoesNotContain(student1, required);
    }
}