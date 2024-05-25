using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Startup;
using Observatory.Framework.Files.Journal.StationServices;

/// <summary>
/// An in-memory database context for Pulsar.
/// </summary>
public class PulsarContext : DbContext
{
    public SqliteConnection Connection { get; private set; }
    
    // Start of game events:
    /**
     *  Materials
        Rank
        Progress
        Reputation
        EngineerProgress
        LoadGame
        --Some time later--
        Statistics
     */
    
    public DbSet<Materials> Materials { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<Progress> Progress { get; set; }
    public DbSet<Reputation> Reputations { get; set; }
    public DbSet<EngineerProgress> EngineerProgress { get; set; }
    public DbSet<LoadGame> LoadGames { get; set; }
    public DbSet<Statistics> Statistics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Connection = new SqliteConnection("Data Source=Journals.sqlite");
        optionsBuilder.UseSqlite(Connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PulsarContext).Assembly);
        base.OnModelCreating(modelBuilder);

        if (Database.ProviderName != "Microsoft.EntityFrameworkCore.Sqlite") return;
        // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
        // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
        // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
        // use the DateTimeOffsetToBinaryConverter
        // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
        // This only supports millisecond precision, but should be sufficient for most use cases.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                           || p.PropertyType == typeof(DateTimeOffset?));
            foreach (var property in properties)
            {
                modelBuilder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(new DateTimeOffsetToBinaryConverter());
            }
        }
    }

    public override void Dispose()
    {
        Connection.Dispose();
        base.Dispose();
    }
}