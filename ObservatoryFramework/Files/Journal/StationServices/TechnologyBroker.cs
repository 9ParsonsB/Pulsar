using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class TechnologyBroker : JournalBase
{
    public override string Event => "TechnologyBroker";
    public string BrokerType { get; init; }
    public ulong MarketID { get; init; }
    public IReadOnlyCollection<ItemName> ItemsUnlocked { get; init; }
    public IReadOnlyCollection<CommodityReward> Commodities { get; init; }
    public IReadOnlyCollection<MaterialReward> Materials { get; init; }
}