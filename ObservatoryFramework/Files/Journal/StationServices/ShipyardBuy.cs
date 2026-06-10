namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when buying a new ship in the shipyard.
/// </summary>
public class ShipyardBuy : JournalBase
{
    public ulong MarketID { get; init; }
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public long ShipPrice { get; init; }
    public string StoreOldShip { get; init; }
    public ulong StoreShipID { get; init; }
    public string SellOldShip { get; init; }
    public ulong SellShipID { get; init; }
    public long SellPrice { get; init; }
}