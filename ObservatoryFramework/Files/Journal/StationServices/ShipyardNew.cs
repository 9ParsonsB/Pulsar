namespace Observatory.Framework.Files.Journal.StationServices;

public class ShipyardNew : JournalBase
{
    public override string Event => "ShipyardNew";
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public ulong NewShipID { get; init; }
}