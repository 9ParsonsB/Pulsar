namespace Pulsar;

using System.Diagnostics.CodeAnalysis;
using Lamar;
using Features;
using Features.Backpack;
using Features.Cargo;
using Features.ModulesInfo;
using Features.Journal;
using Features.Market;
using Features.NavRoute;
using Features.Outfitting;
using Features.ShipLocker;
using Features.Shipyard;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class PulsarServiceRegistry : ServiceRegistry
{
    public PulsarServiceRegistry()
    {
        For<IFileHandlerService>().Use<FileHandlerService>();
        For<IStatusService>().Use<StatusService>();
        For<IModulesInfoService>().Use<ModulesInfoService>();
        For<ICargoService>().Use<CargoService>();
        For<IJournalService>().Use<JournalService>();
        For<IJournalStore>().Use<JournalStore>().Singleton();
        For<IShipLockerService>().Use<ShipLockerService>();
        For<IShipyardService>().Use<ShipyardService>();
        For<IMarketService>().Use<MarketService>();
        For<IBackpackService>().Use<BackpackService>();
        For<INavRouteService>().Use<NavRouteService>();
        For<IOutfittingService>().Use<OutfittingService>();
    }
}