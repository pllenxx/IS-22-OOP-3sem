using Business.Dto;
using DataAccess.Entities;

namespace Business.Mapping;

public static class MessageMapping
{
    public static MessageDto AsDto(this Message message)
        => new MessageDto(message.Id, message.Context, message.Source.ToString());
}