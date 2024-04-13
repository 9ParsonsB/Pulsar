namespace Observatory.Framework.Files.Journal.StationServices;

public class ClearImpound : JournalBase
{
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public ulong ShipID { get; init; }
    public ulong ShipMarketID { get; init; }
    public ulong MarketID { get; init; }
}