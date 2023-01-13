using Business.Services;
using Business.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task CreateReport([FromBody] ReportDto reportDto, [FromQuery] Guid requiredMessageSourceId)
    {
        await _reportService.CreateReport(reportDto.AuthorId, requiredMessageSourceId, CancellationToken);
    }
}