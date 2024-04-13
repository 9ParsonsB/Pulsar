namespace Observatory.Framework.Files.Journal.StationServices;

public class ShipyardNew : JournalBase
{
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public ulong NewShipID { get; init; }
}