namespace Observatory.Framework.Files.Journal.StationServices;

public class SellShipOnRebuy : JournalBase
{
    public override string Event => "SellShipOnRebuy";
    public string ShipType { get; init; }
    public string System { get; init; }
    public ulong SellShipId { get; init; }
    public long ShipPrice { get; init; }
}