using Business.Dto;
using DataAccess.Enums;
using Business.Exceptions;
using Business.Extensions;
using Business.Mapping;
using DataAccess;
using DataAccess.Entities;

namespace Business.Services.Implementations;

public class ReportService : IReportService
{
    private readonly DatabaseContext _context;

    public ReportService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<ReportDto> CreateReport(Guid authorId, Guid messageSourceId, CancellationToken cancellationToken)
    {
        var author = await _context.Employees.GetEntityAsync(authorId, cancellationToken);
        var messageSource = await _context.MessageSources.GetEntityAsync(messageSourceId, cancellationToken);
        if (author.Type is not EmployeeType.Supervisor)
            throw new MessageProcessingSystemException("Employee cannot create a report");
        var report = new Report(Guid.NewGuid(), author, DateTime.Now);
        report.AmountOfProcessedMessages = 
            author.Subordinates.SelectMany(employee => employee.Messages).Count(message => message.Status == MessageStatus.Processed);
        report.AmountOfMessagesToSpecificSource =
            author.Subordinates.SelectMany(employee => employee.Messages)
                .Count(message => message.Source == messageSource);
        report.Statistics =
            author.Messages.Select(message =>
                message.TimeOfProcession > DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)))
                .Count();
        
        _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();

        return report.AsDto();
    }
}