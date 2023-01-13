using DataAccess.Enums;

namespace DataAccess.Entities;

public class Report
{
    public Report(Guid id, Employee author, DateTime creationTime)
    {
        Id = id;
        CreationTime = creationTime;
        Author = author;
        AmountOfProcessedMessages = 0;
        AmountOfMessagesToSpecificSource = 0;
        Statistics = 0;
    }
    
    protected Report() {}
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public virtual Employee Author { get; set; }
    public int AmountOfProcessedMessages { get; set; }
    public int AmountOfMessagesToSpecificSource { get; set; }
    public double Statistics { get; set; }
}