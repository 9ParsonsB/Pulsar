namespace Pulsar.Features;

using Backpack;
using Cargo;
using Market;
using ModulesInfo;
using NavRoute;
using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Odyssey;
using Outfitting;
using ShipLocker;
using Shipyard;

public class EventsHub(
    IJournalService journalService,
    IStatusService statusService,
    IOutfittingService outfittingService,
    IShipyardService shipyardService,
    INavRouteService navRouteService,
    IMarketService marketService,
    IModulesInfoService modulesInfoService,
    ICargoService cargoService,
    IBackpackService backpackService,
    IShipLockerService shipLockerService) : Hub<IEventsHub>
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        await Clients.Caller.StatusUpdated(await statusService.Get());
        await Clients.Caller.OutfittingUpdated(await outfittingService.Get());
        await Clients.Caller.ShipyardUpdated(await shipyardService.Get());
        await Clients.Caller.NavRouteUpdated(await navRouteService.Get());
        await Clients.Caller.MarketUpdated(await marketService.Get());
        await Clients.Caller.ModuleInfoUpdated(await modulesInfoService.Get());
        await Clients.Caller.CargoUpdated(await cargoService.Get());
        await Clients.Caller.BackpackUpdated(await backpackService.Get());
        await Clients.Caller.ShipLockerUpdated(await shipLockerService.Get());

        await Clients.Caller.JournalUpdated(await journalService.GetLastStartupEvents());
        var state = await journalService.GetLatestState();
        if (state.Any()) await Clients.Caller.JournalUpdated(state);
    }

    public async Task Status()
    {
        var status = await statusService.Get();
        await Clients.Caller.StatusUpdated(status);
    }
}

public interface IEventsHub
{
    Task StatusUpdated(Observatory.Framework.Files.Status status);

    Task OutfittingUpdated(OutfittingFile outfitting);

    Task ShipyardUpdated(ShipyardFile shipyard);

    Task NavRouteUpdated(NavRouteFile navRoute);

    Task MarketUpdated(MarketFile market);

    Task JournalUpdated(List<JournalBase> journals);

    Task ModuleInfoUpdated(ModuleInfoFile moduleInfo);

    Task FleetCarrierUpdated(FCMaterialsFile fleetCarrier);

    Task CargoUpdated(CargoFile cargo);

    Task BackpackUpdated(BackpackFile backpack);

    Task ShipLockerUpdated(ShipLockerMaterials shipLocker);
}