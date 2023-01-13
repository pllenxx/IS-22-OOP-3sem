using Business.Dto;
using DataAccess.Entities;

namespace Business.Mapping;

public static class ReportMapping
{
    public static ReportDto AsDto(this Report report)
        => new ReportDto(report.Id, report.Author.Id);
}