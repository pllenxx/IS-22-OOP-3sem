using Isu.Tools;

namespace Isu.Models;

public class CourseNumber
{
    private const int _firstCourse = 1;
    private const int _lastCourse = 4;
    private int _number;

    public CourseNumber(int number)
    {
        if (number < _firstCourse || number > _lastCourse)
            throw new GroupException("This person is not an undergraduate student");
        _number = number;
    }
}