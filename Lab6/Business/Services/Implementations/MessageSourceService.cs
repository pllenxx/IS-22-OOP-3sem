using Business.Dto;
using DataAccess.Enums;
using Business.Exceptions;
using Business.Extensions;
using Business.Mapping;
using DataAccess;
using DataAccess.Entities;

namespace Business.Services.Implementations;

public class MessageSourceService : IMessageSourceService
{
    private readonly DatabaseContext _context;

    public MessageSourceService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<MessageSourceDto> CreateMessageSource(MessageSourceType type, CancellationToken cancellationToken)
    {
        var source = new MessageSource(Guid.NewGuid(), type);
        await _context.MessageSources.AddAsync(source);
        await _context.SaveChangesAsync(cancellationToken);

        return source.AsDto();
    }

    public async Task SendMessage(string context, Guid senderId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(context))
            throw new MessageProcessingSystemException("Message is meaningless");
        var source = await _context.MessageSources.GetEntityAsync(senderId, cancellationToken);
        var message = new Message(Guid.NewGuid(), context, source);

        source.SentMessages.Add(message);
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync(cancellationToken);
    }
}