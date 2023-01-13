using Business.Dto;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class MessageSourceController : ControllerBase
{
    private readonly IMessageSourceService _messageSourceService;

    public MessageSourceController(IMessageSourceService messageSourceService)
    {
        _messageSourceService = messageSourceService;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<ActionResult<MessageSourceDto>> Create([FromBody] MessageSourceDto messageSourceDto)
    {
        var source = await _messageSourceService.CreateMessageSource(messageSourceDto.Type, CancellationToken);
        return Ok(source);
    }

    [HttpPost("send-message-to-system")]
    public async Task SendMessage([FromRoute] Guid senderId, string context)
    {
        await _messageSourceService.SendMessage(context, senderId, CancellationToken);
    }
}