using DataAccess.Enums;
namespace DataAccess.Entities;

public class Message
{
    public Message(Guid id, string context, MessageSource source)
    {
        Id = id;
        Status = MessageStatus.New;
        Source = source;
        Context = context;
    }

    protected Message() {}
    public Guid Id { get; set; }
    public string Context { get; set; }
    public virtual MessageSource Source { get; set; }
    public MessageStatus Status { get; set; }
    public DateTime TimeOfProcession { get; set; }
}