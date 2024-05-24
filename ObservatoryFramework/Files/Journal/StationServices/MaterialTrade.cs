using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class MaterialTrade : JournalBase
{
    public override string Event => "MaterialTrade";
    public ulong MarketID { get; init; }
    public string TraderType { get; init; }
    public TradeDetail Paid { get; init; }
    public TradeDetail Received { get; init; }
}