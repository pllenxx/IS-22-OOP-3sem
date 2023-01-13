using Business.Dto;
using DataAccess.Entities;

namespace Business.Mapping;

public static class MessageSourceMapping
{
    public static MessageSourceDto AsDto(this MessageSource messageSource)
        => new MessageSourceDto(messageSource.Id, messageSource.SourceType);
}