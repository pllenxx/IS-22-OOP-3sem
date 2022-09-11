using Isu.Tools;

namespace Isu.Models;

public class CourseNumber
{
    private int _number;

    public CourseNumber(int number)
    {
        if (number < 1 || number > 4)
            throw new GroupException("This person is not an undergraduate student!");
        _number = number;
    }
}