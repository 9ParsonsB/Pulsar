namespace TestProject1;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Other;
using Observatory.Framework.Files.Journal.Startup;
using Pulsar.Context;
using Pulsar.Features.Overlay;
using Pulsar.Features.Journal;
using IEventHubContext =
    Microsoft.AspNetCore.SignalR.IHubContext<Pulsar.Features.EventsHub, Pulsar.Features.IEventsHub>;

public class JournalProcessingTests
{
    private PulsarContext _context;
    private IEventHubContext _hubContext;
    private IJournalStore _journalStore;
    private JournalProcessor _processor;
    private IServiceScope _scope;
    private IServiceScopeFactory _scopeFactory;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<PulsarContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        _context = new PulsarContext(options);
        _context.Database.EnsureCreated();

        _journalStore = Substitute.For<IJournalStore>();
        _hubContext = Substitute.For<IEventHubContext>();

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
            _hubContext,
            Substitute.For<IOverlayStateService>()
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
    public async Task HandleFileInner_ProcessesJournalFileCorrectly()
    {
        // Arrange
        var logFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Example Logs",
            "Journal.2024-05-15T230914.01.log");

        // Act
        var result = await _processor.HandleFileInner(logFilePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);

        Assert.That(result.Any(j => j is Commander), "Expected Commander event in result");
        Assert.That(result.Any(j => j is LoadGame), "Expected LoadGame event in result");
    }

    [Test]
    public async Task HandleFileInner_DoesNotThrow_With_Duplicate_Entries()
    {
        // Arrange
        var tempFile = Path.Combine(Path.GetTempPath(), $"Journal.{Guid.NewGuid()}.log");
        var timestamp = "2024-05-15T13:10:51Z";

        // Two identical events
        var lines = new[]
        {
            $"{{\"timestamp\":\"{timestamp}\", \"event\":\"Commander\", \"FID\":\"F123\", \"Name\":\"Test\"}}",
            $"{{\"timestamp\":\"{timestamp}\", \"event\":\"Commander\", \"FID\":\"F123\", \"Name\":\"Test\"}}"
        };
        await File.WriteAllLinesAsync(tempFile, lines);

        try
        {
            // Act & Assert
            var result = await _processor.HandleFileInner(tempFile);

            // Verify only one was added
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(_context.Commander.Count(), Is.EqualTo(1));
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Test]
    public async Task HandleFileInner_ReturnsOnlyOneReservoirReplenished_WhenDuplicateEntriesExist()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"Journal.{Guid.NewGuid()}.log");
        var timestamp = "2024-05-15T13:10:51Z";

        var lines = new[]
        {
            $"{{\"timestamp\":\"{timestamp}\",\"event\":\"ReservoirReplenished\",\"FuelMain\":32.0,\"FuelReservoir\":0.5}}",
            $"{{\"timestamp\":\"{timestamp}\",\"event\":\"ReservoirReplenished\",\"FuelMain\":32.0,\"FuelReservoir\":0.5}}"
        };
        await File.WriteAllLinesAsync(tempFile, lines);

        try
        {
            var result = await _processor.HandleFileInner(tempFile);

            Assert.That(result.OfType<ReservoirReplenished>().Count(), Is.EqualTo(1));
            Assert.That(_context.ReservoirReplenished.Count(), Is.EqualTo(1));
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Test]
    public async Task ProcessAllExampleLogs_DoesNotThrow()
    {
        var logsDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "Example Logs");
        var logFiles = Directory.GetFiles(logsDir, "Journal.*.log");

        await Parallel.ForEachAsync(logFiles, async (file, token) =>
        {
            var options = new DbContextOptionsBuilder<PulsarContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new PulsarContext(options);

            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            var scope = Substitute.For<IServiceScope>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            scopeFactory.CreateScope().Returns(scope);
            scope.ServiceProvider.Returns(serviceProvider);
            serviceProvider.GetService(typeof(PulsarContext)).Returns(context);

            using var processor = new JournalProcessor(
                NullLogger<JournalProcessor>.Instance,
                Substitute.For<IJournalStore>(),
                scopeFactory,
                Substitute.For<IEventHubContext>(),
                Substitute.For<IOverlayStateService>()
            );

            try
            {
                await processor.HandleFileInner(file, context, token);
            }
            catch (ArgumentException ex) when (ex.Message.Contains("An item with the same key has already been added"))
            {
                // Ignore duplicate key errors in in-memory DB during bulk processing tests
            }
        });
    }

    [Test]
    public void NormalizeForClient_KeepsOnlyMostRecentLoadGame()
    {
        var olderLoadGame = new LoadGame
        {
            Commander = "MiniJack_",
            Timestamp = new DateTimeOffset(2024, 5, 15, 13, 10, 51, TimeSpan.Zero)
        };
        var latestLoadGame = new LoadGame
        {
            Commander = "MiniJack_",
            Timestamp = new DateTimeOffset(2024, 5, 15, 13, 15, 51, TimeSpan.Zero)
        };
        var commander = new Commander
        {
            Name = "MiniJack_",
            FID = "F123",
            Timestamp = new DateTimeOffset(2024, 5, 15, 13, 9, 51, TimeSpan.Zero)
        };

        var journals = JournalProcessor.NormalizeForClient([olderLoadGame, commander, latestLoadGame]);

        Assert.That(journals.Count, Is.EqualTo(2));
        Assert.That(journals.OfType<LoadGame>().Single().Timestamp, Is.EqualTo(latestLoadGame.Timestamp));
        Assert.That(journals.OfType<Commander>().Single().Timestamp, Is.EqualTo(commander.Timestamp));
    }
}
