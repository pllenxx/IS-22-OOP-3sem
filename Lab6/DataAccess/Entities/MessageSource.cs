using DataAccess.Enums;

namespace DataAccess.Entities;

public class MessageSource
{
    public MessageSource(Guid id, MessageSourceType sourceType)
    {
        Id = id;
        SourceType = sourceType;
    }
    
    protected MessageSource() {}
    public Guid Id { get; set; }
    public MessageSourceType SourceType { get; set; }
    public virtual Account Account { get; set; }
    public virtual ICollection<Message> SentMessages { get; set; }
}