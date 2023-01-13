using Business.Dto;
using Business.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/employee")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost("create-employee")]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] EmployeeDto employeeDto)
    {
        var employee = await _employeeService.CreateEmployee(employeeDto.Name, employeeDto.Position, CancellationToken);
        return Ok(employee);
    }

    [HttpPut("set-supervisor")]
    public async Task SetAsSupervisor([FromRoute] Guid supervisorId, [FromQuery] Guid subordinateId)
    {
        await _employeeService.SetSupervisor(supervisorId, subordinateId, CancellationToken);
    }
}