namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when after a new ship has been purchased.
/// </summary>
public class ShipyardNew : JournalBase
{
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public ulong NewShipID { get; init; }
}