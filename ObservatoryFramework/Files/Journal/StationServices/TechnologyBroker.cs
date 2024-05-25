using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class TechnologyBroker : JournalBase
{
    public override string Event => "TechnologyBroker";
    public string BrokerType { get; init; }
    public ulong MarketID { get; init; }
    public List<ItemName> ItemsUnlocked { get; init; }
    public List<CommodityReward> Commodities { get; init; }
    public List<MaterialReward> Materials { get; init; }
}