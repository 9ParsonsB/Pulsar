namespace Observatory.Framework.Files.Journal.StationServices;

public class ShipyardTransfer : JournalBase
{
    public override string Event => "ShipyardTransfer";
    public ulong MarketID { get; init; }
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public ulong ShipID { get; init; }
    public string System { get; init; }
    public ulong ShipMarketID { get; init; }
    public float Distance { get; init; }
    public int TransferPrice { get; init; }
    public long TransferTime { get; init; }
}