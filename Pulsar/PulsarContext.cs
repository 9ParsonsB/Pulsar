using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Observatory.Framework.Files.Journal;

/// <summary>
/// An in-memory database context for Pulsar.
/// </summary>
public class PulsarContext : DbContext
{
    public SqliteConnection Connection { get; private set;  }
    
    public DbSet<JournalBase> Journals { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Connection = new SqliteConnection("Data Source=:memory:");
        optionsBuilder.UseSqlite(Connection);
    }

    public override void Dispose()
    {
        Connection.Dispose();
        base.Dispose();
    }
}