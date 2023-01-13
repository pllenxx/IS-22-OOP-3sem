using DataAccess.Enums;

namespace Business.Dto;

public record EmployeeDto(Guid Id, string Name, EmployeeType Position);