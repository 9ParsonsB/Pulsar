namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when When selling a stored ship to raise funds when on insurance/rebuy screen.
/// </summary>
public class SellShipOnRebuy : JournalBase
{
    public string ShipType { get; init; }
    public string System { get; init; }
    public ulong SellShipId { get; init; }
    public long ShipPrice { get; init; }
}