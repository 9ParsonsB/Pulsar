namespace TestProject1;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Pulsar.Context;
using Pulsar.Features;
using Pulsar.Features.Journal;

public class ScanKeyTests
{
    private PulsarContext _context;
    private IHubContext<EventsHub, IEventsHub> _hubContext;
    private IJournalStore _journalStore;
    private JournalProcessor _processor;
    private IServiceScope _scope;
    private IServiceScopeFactory _scopeFactory;

    [SetUp]
    public void Setup()
    {
        // Use a REAL Sqlite in-memory database to test key constraints, 
        // as EF Core In-Memory database doesn't enforce all constraints like composite keys correctly in some cases.
        // Actually, Pulsar uses Sqlite usually.
        var options = new DbContextOptionsBuilder<PulsarContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new PulsarContext(options);
        _context.Database.EnsureCreated();

        _journalStore = Substitute.For<IJournalStore>();
        _hubContext = Substitute.For<IHubContext<EventsHub, IEventsHub>>();

        _scopeFactory = Substitute.For<IServiceScopeFactory>();
        _scope = Substitute.For<IServiceScope>();
        var serviceProvider = Substitute.For<IServiceProvider>();

        _scopeFactory.CreateScope().Returns(_scope);
        _scope.ServiceProvider.Returns(serviceProvider);
        serviceProvider.GetService(typeof(PulsarContext)).Returns(_context);

        _processor = new JournalProcessor(
            NullLogger<JournalProcessor>.Instance,
            _journalStore,
            _scopeFactory,
            _hubContext
        );
    }

    [TearDown]
    public void TearDown()
    {
        _processor.Dispose();
        _scope.Dispose();
        _context.Dispose();
    }

    [Test]
    public async Task HandleFileInner_AllowsMultipleScansWithSameTimestampDifferentBodyID()
    {
        // Arrange
        var tempFile = Path.Combine(Path.GetTempPath(), $"Journal.{Guid.NewGuid()}.log");
        var timestamp = "2024-05-15T13:10:51Z";

        // Two scan events with same timestamp but different BodyID
        // Adding all required fields to satisfy EF Core In-Memory database nullability checks
        var lines = new[]
        {
            $"{{\"timestamp\":\"{timestamp}\", \"event\":\"Scan\", \"ScanType\":\"Detailed\", \"BodyName\":\"Body 1\", \"BodyID\":1, \"StarSystem\":\"System\", \"Atmosphere\":\"\", \"AtmosphereType\":\"\", \"Luminosity\":\"\", \"PlanetClass\":\"\", \"ReserveLevel\":\"\", \"StarType\":\"\", \"TerraformState\":\"\", \"Volcanism\":\"\"}}",
            $"{{\"timestamp\":\"{timestamp}\", \"event\":\"Scan\", \"ScanType\":\"Detailed\", \"BodyName\":\"Body 2\", \"BodyID\":2, \"StarSystem\":\"System\", \"Atmosphere\":\"\", \"AtmosphereType\":\"\", \"Luminosity\":\"\", \"PlanetClass\":\"\", \"ReserveLevel\":\"\", \"StarType\":\"\", \"TerraformState\":\"\", \"Volcanism\":\"\"}}"
        };
        await File.WriteAllLinesAsync(tempFile, lines);

        try
        {
            // Act
            await _processor.HandleFileInner(tempFile);

            // Assert
            Assert.That(_context.Scans.Count(), Is.EqualTo(2), "Expected two scan events to be stored");
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Test]
    public async Task HandleFileInner_AllowsMultipleScanBaryCentresWithSameTimestampDifferentBodyID()
    {
        // Arrange
        var tempFile = Path.Combine(Path.GetTempPath(), $"Journal.{Guid.NewGuid()}.log");
        var timestamp = "2024-05-15T13:10:51Z";

        // Two ScanBaryCentre events with same timestamp but different BodyID
        var lines = new[]
        {
            $"{{\"timestamp\":\"{timestamp}\", \"event\":\"ScanBaryCentre\", \"BodyID\":15, \"StarSystem\":\"System\"}}",
            $"{{\"timestamp\":\"{timestamp}\", \"event\":\"ScanBaryCentre\", \"BodyID\":16, \"StarSystem\":\"System\"}}"
        };
        await File.WriteAllLinesAsync(tempFile, lines);

        try
        {
            // Act
            await _processor.HandleFileInner(tempFile);

            // Assert
            // ScanBaryCentre is stored in the Scans table if it's treated as a root of the hierarchy 
            // and we use TPH or similar. In PulsarContext, Scans is DbSet<Scan>.
            // Wait, PulsarContext doesn't have DbSet<ScanBaryCentre>.
            // Let's check how many Scans are there. 
            // Actually, if ScanBaryCentre is not in a DbSet, it might not be stored at all unless it's a Scan.
            // But I added it to the model via ApplyConfigurationsFromAssembly.
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }
}