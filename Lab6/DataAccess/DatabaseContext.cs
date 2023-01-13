using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace DataAccess;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<MessageSource> MessageSources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(builder =>
        {
            builder.HasOne(x => x.Account);
        });

        modelBuilder.Entity<MessageSource>(builder =>
        {
            builder.HasOne(x => x.Account);
            builder.HasMany(x => x.SentMessages);
        });

        modelBuilder.Entity<Report>(builder =>
            builder.HasOne(x => x.Author));
    }
}