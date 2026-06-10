namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when selling a ship stored in the shipyard.
/// </summary>
public class ShipyardSell : JournalBase
{
    public ulong MarketID { get; init; }
    public string ShipType { get; init; }
    public ulong SellShipID { get; init; }
    public long ShipPrice { get; init; }
    public string System { get; init; }
}