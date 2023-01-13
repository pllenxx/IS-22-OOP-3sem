using Business.Dto;
using DataAccess.Entities;

namespace Business.Services;

public interface IReportService
{
    Task<ReportDto> CreateReport(Guid authorId, Guid messageSourceId, CancellationToken cancellationToken);
}