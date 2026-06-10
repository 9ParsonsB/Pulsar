namespace Observatory.Framework.Files.Journal.StationServices;

using ParameterTypes;

/// <summary>
///     Written when using the Technology Broker to unlock new purchasable technology.
/// </summary>
public class TechnologyBroker : JournalBase
{
    public string BrokerType { get; init; }
    public ulong MarketID { get; init; }
    public List<ItemName> ItemsUnlocked { get; init; }
    public List<CommodityReward> Commodities { get; init; }
    public List<MaterialReward> Materials { get; init; }
}