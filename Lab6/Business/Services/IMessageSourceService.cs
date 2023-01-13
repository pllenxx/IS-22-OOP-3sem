using Business.Dto;
using DataAccess.Enums;

namespace Business.Services;

public interface IMessageSourceService
{
    Task<MessageSourceDto> CreateMessageSource(MessageSourceType type, CancellationToken cancellationToken);
    Task SendMessage(string context, Guid senderId, CancellationToken cancellationToken);
}