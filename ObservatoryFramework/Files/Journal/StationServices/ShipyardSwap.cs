namespace Observatory.Framework.Files.Journal.StationServices;

public class ShipyardSwap : JournalBase
{
    public override string Event => "ShipyardSwap";
    public ulong MarketID { get; init; }
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public ulong ShipID { get; init; }
    public string StoreOldShip { get; init; }
    public ulong StoreShipID { get; init; }
    public string SellOldShip { get; init; }
    public ulong SellShipID { get; init; }
}