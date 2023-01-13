using Business.Dto;
using DataAccess.Enums;
using DataAccess.Entities;

namespace Business.Services;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateEmployee(string name, EmployeeType type, CancellationToken cancellationToken);
    Task SetSupervisor(Guid supervisorId, Guid subordinateId, CancellationToken cancellationToken);
    Task UploadMessagesToSystem(ICollection<Message> messages, CancellationToken cancellationToken);
    Task ProcessMessage(Guid employeeId, Guid messageId, CancellationToken cancellationToken);
}