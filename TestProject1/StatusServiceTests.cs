namespace TestProject1;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Observatory.Framework.Files.Journal.Other;
using Observatory.Framework.Files.Journal.Startup;
using Pulsar;
using Pulsar.Context;
using Pulsar.Features.Overlay;
using Pulsar.Features.Status;
using IEventHubContext =
    Microsoft.AspNetCore.SignalR.IHubContext<Pulsar.Features.EventsHub, Pulsar.Features.IEventsHub>;

public class StatusServiceTests
{
    private PulsarContext _context;
    private string _tempDir;

    [SetUp]
    public void Setup()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDir);

        var options = new DbContextOptionsBuilder<PulsarContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new PulsarContext(options);
        _context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
    }

    [Test]
    public async Task Get_FallsBackToJournalData_WhenStatusFileMissing()
    {
        var loadGameTimestamp = DateTimeOffset.Parse("2026-06-10T08:32:07Z");
        var cargoTimestamp = DateTimeOffset.Parse("2026-06-10T08:32:24Z");
        var reservoirTimestamp = DateTimeOffset.Parse("2026-06-10T08:57:15Z");

        _context.LoadGames.Add(new LoadGame
        {
            Timestamp = loadGameTimestamp,
            Commander = "MiniJack_",
            Credits = 126210596,
            FuelLevel = 113.777534,
            FuelCapacity = 128
        });
        _context.Cargo.Add(new Cargo
        {
            Timestamp = cargoTimestamp,
            Vessel = "Ship",
            Count = 15,
            Inventory = []
        });
        _context.ReservoirReplenished.Add(new ReservoirReplenished
        {
            Timestamp = reservoirTimestamp,
            FuelMain = 111.497536f,
            FuelReservoir = 1.14f
        });
        await _context.SaveChangesAsync();

        var service = new StatusService(
            NullLogger<StatusService>.Instance,
            Options.Create(new PulsarConfiguration { JournalDirectory = _tempDir }),
            Substitute.For<IEventHubContext>(),
            _context,
            Substitute.For<IOverlayStateService>());

        var status = await service.Get();

        Assert.That(status.Timestamp, Is.EqualTo(reservoirTimestamp));
        Assert.That(status.Balance, Is.EqualTo(126210596));
        Assert.That(status.Cargo, Is.EqualTo(15));
        Assert.That(status.Fuel?.FuelMain, Is.EqualTo(111.497536f).Within(0.0001));
        Assert.That(status.Fuel?.FuelReservoir, Is.EqualTo(1.14f).Within(0.0001));
    }
}
