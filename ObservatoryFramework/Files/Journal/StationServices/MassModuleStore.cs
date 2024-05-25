using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class MassModuleStore : JournalBase
{
    public override string Event => "MassModuleStore";
    public ulong MarketID { get; init; }
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
    public IReadOnlyCollection<Item> Items { get; init; }
}