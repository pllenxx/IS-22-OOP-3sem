using DataAccess.Enums;

namespace Business.Dto;

public record MessageSourceDto(Guid Id, MessageSourceType Type);