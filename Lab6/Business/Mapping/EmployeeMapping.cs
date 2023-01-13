using Business.Dto;
using DataAccess.Entities;

namespace Business.Mapping;

public static class EmployeeMapping
{
    public static EmployeeDto AsDto(this Employee employee)
        => new EmployeeDto(employee.Id, employee.Name, employee.Type);
}