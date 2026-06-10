namespace TestProject1;

using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Odyssey;
using Observatory.Framework.Files.Journal.Startup;
using Pulsar.Features;
using Pulsar.Features.Backpack;
using Pulsar.Features.Cargo;
using Pulsar.Features.Journal;
using Pulsar.Features.Market;
using Pulsar.Features.ModulesInfo;
using Pulsar.Features.NavRoute;
using Pulsar.Features.Outfitting;
using Pulsar.Features.ShipLocker;
using Pulsar.Features.Shipyard;
using Pulsar.Features.Status;

public class EventsHubTests
{
    [Test]
    public async Task OnConnectedAsync_SendsCurrentSnapshotsToCaller()
    {
        var journalService = Substitute.For<IJournalService>();
        var statusService = Substitute.For<IStatusService>();
        var outfittingService = Substitute.For<IOutfittingService>();
        var shipyardService = Substitute.For<IShipyardService>();
        var navRouteService = Substitute.For<INavRouteService>();
        var marketService = Substitute.For<IMarketService>();
        var modulesInfoService = Substitute.For<IModulesInfoService>();
        var cargoService = Substitute.For<ICargoService>();
        var backpackService = Substitute.For<IBackpackService>();
        var shipLockerService = Substitute.For<IShipLockerService>();

        var status = new Status();
        var outfitting = new OutfittingFile();
        var shipyard = new ShipyardFile();
        var navRoute = new NavRouteFile();
        var market = new MarketFile();
        var modulesInfo = new ModuleInfoFile();
        var cargo = new CargoFile();
        var backpack = new BackpackFile();
        var shipLocker = new ShipLockerMaterials();
        var startupEvents = new List<JournalBase> { new LoadGame { Commander = "MiniJack_" } };
        var latestState = new List<JournalBase> { new LoadGame { Commander = "MiniJack_" } };

        statusService.Get().Returns(status);
        outfittingService.Get().Returns(outfitting);
        shipyardService.Get().Returns(shipyard);
        navRouteService.Get().Returns(navRoute);
        marketService.Get().Returns(market);
        modulesInfoService.Get().Returns(modulesInfo);
        cargoService.Get().Returns(cargo);
        backpackService.Get().Returns(backpack);
        shipLockerService.Get().Returns(shipLocker);
        journalService.GetLastStartupEvents().Returns(startupEvents);
        journalService.GetLatestState().Returns(latestState);

        var caller = Substitute.For<IEventsHub>();
        var clients = Substitute.For<IHubCallerClients<IEventsHub>>();
        clients.Caller.Returns(caller);

        var hub = new EventsHub(
            journalService,
            statusService,
            outfittingService,
            shipyardService,
            navRouteService,
            marketService,
            modulesInfoService,
            cargoService,
            backpackService,
            shipLockerService)
        {
            Clients = clients,
            Context = Substitute.For<HubCallerContext>()
        };

        await hub.OnConnectedAsync();

        await caller.Received(1).StatusUpdated(status);
        await caller.Received(1).OutfittingUpdated(outfitting);
        await caller.Received(1).ShipyardUpdated(shipyard);
        await caller.Received(1).NavRouteUpdated(navRoute);
        await caller.Received(1).MarketUpdated(market);
        await caller.Received(1).ModuleInfoUpdated(modulesInfo);
        await caller.Received(1).CargoUpdated(cargo);
        await caller.Received(1).BackpackUpdated(backpack);
        await caller.Received(1).ShipLockerUpdated(shipLocker);
        await caller.Received(1).JournalUpdated(startupEvents);
        await caller.Received(1).JournalUpdated(latestState);
    }
}