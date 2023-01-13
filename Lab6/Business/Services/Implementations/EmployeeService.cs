using Business.Dto;
using DataAccess.Enums;
using Business.Exceptions;
using Business.Extensions;
using Business.Mapping;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enums;

namespace Business.Services.Implementations;

internal class EmployeeService : IEmployeeService
{
    private readonly DatabaseContext _context;

    public EmployeeService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<EmployeeDto> CreateEmployee(string name, EmployeeType type, CancellationToken cancellationToken)
    {
        var employee = new Employee(Guid.NewGuid(), name, type);

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return employee.AsDto();
    }

    public async Task SetSupervisor(Guid supervisorId, Guid subordinateId, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.GetEntityAsync(subordinateId, cancellationToken);
        var chief = await _context.Employees.GetEntityAsync(supervisorId, cancellationToken);

        if (chief.Type is not EmployeeType.Supervisor)
            throw new MessageProcessingSystemException("This employee doesn't have supervisor's rights");
        chief.Subordinates.Add(employee);
        employee.ChiefId = chief.Id;
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UploadMessagesToSystem(ICollection<Message> messages, CancellationToken cancellationToken)
    {
        foreach (var message in messages)
        {
            message.Status = MessageStatus.Received;
        }
        
        await _context.Messages.AddRangeAsync(messages);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ProcessMessage(Guid employeeId, Guid messageId, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.GetEntityAsync(employeeId, cancellationToken);
        var message = await _context.Messages.GetEntityAsync(messageId, cancellationToken);

        message.Status = MessageStatus.Processed;
        message.TimeOfProcession = DateTime.Now;
        employee.Messages.Add(message);

        await _context.SaveChangesAsync(cancellationToken);
    }
}