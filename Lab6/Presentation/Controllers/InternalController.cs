using Business.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class InternalController : ControllerBase
{
    private readonly IAccountService _accountService;

    public InternalController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost("create-employee-account")]
    public async Task<ActionResult> CreateEmployeeAccount([FromQuery] string name, [FromQuery] string password)
    {
        var accountDto = await _accountService.CreateAccountForEmployee(name, password, CancellationToken);
        return Ok(accountDto);
    }
    
    [HttpPost("create-message-source-account")]
    public async Task<IActionResult> CreateSupervisorAccount([FromQuery] string name, [FromQuery] string password)
    {
        var accountDto = await _accountService.CreateAccountForSource(name, password, CancellationToken);
        return Ok(accountDto);
    }

    [HttpPost("grant-supervisor-rights")]
    public async Task GrantSupervisorRights([FromQuery] Guid id)
    {
        await _accountService.GrantSupervisorRights(id, CancellationToken);
    }
}