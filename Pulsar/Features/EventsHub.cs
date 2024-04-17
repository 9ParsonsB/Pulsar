namespace Pulsar.Features;

using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;
using Microsoft.AspNetCore.SignalR;

public class EventsHub : Hub<IEventHub>
{
    public async Task StatusUpdated(Observatory.Framework.Files.Status status) => await Clients.All.StatusUpdated(status);
    
    public async Task OutfittingUpdated(OutfittingFile outfitting) => await Clients.All.OutfittingUpdated(outfitting);
    
    public async Task ShipyardUpdated(ShipyardFile shipyard) => await Clients.All.ShipyardUpdated(shipyard);
    
    public async Task NavRouteUpdated(NavRouteFile navRoute) => await Clients.All.NavRouteUpdated(navRoute);
    
    public async Task MarketUpdated(MarketFile market) => await Clients.All.MarketUpdated(market);
    
    public async Task JournalUpdated(IReadOnlyCollection<JournalBase> journals) => await Clients.All.JournalUpdated(journals);
    
    public async Task ModuleInfoUpdated(ModuleInfoFile moduleInfo) => await Clients.All.ModuleInfoUpdated(moduleInfo);
    
    public async Task FleetCarrierUpdated(FCMaterialsFile fleetCarrier) => await Clients.All.FleetCarrierUpdated(fleetCarrier);
    
    public async Task CargoUpdated(CargoFile cargo) => await Clients.All.CargoUpdated(cargo);
    
    public async Task BackpackUpdated(BackpackFile backpack) => await Clients.All.BackpackUpdated(backpack);
}

public interface IEventHub
{
    Task StatusUpdated(Observatory.Framework.Files.Status status);
    
    Task OutfittingUpdated(OutfittingFile outfitting);
    
    Task ShipyardUpdated(ShipyardFile shipyard);
    
    Task NavRouteUpdated(NavRouteFile navRoute);
    
    Task MarketUpdated(MarketFile market);
    
    Task JournalUpdated(IReadOnlyCollection<JournalBase> journals);
    
    Task ModuleInfoUpdated(ModuleInfoFile moduleInfo);
    
    Task FleetCarrierUpdated(FCMaterialsFile fleetCarrier);
    
    Task CargoUpdated(CargoFile cargo);
    
    Task BackpackUpdated(BackpackFile backpack);
}