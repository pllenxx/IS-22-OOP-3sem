using DataAccess.Enums;

namespace DataAccess.Entities;

public class Employee
{
    public Employee(Guid id, string name, EmployeeType type)
    {
        Id = id;
        Name = name;
        Subordinates = new List<Employee>();
        Messages = new List<Message>();
        Type = type;
    }

    protected Employee() {}
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual Account Account { get; set; }
    public EmployeeType Type { get; set; }
    public Guid? ChiefId { get; set; }
    public virtual ICollection<Employee> Subordinates { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
}