namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when assigning a name to the ship in Starport Services.
/// </summary>
public class SetUserShipName : JournalBase
{
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
    public string UserShipName { get; init; }
    public string UserShipId { get; init; }
}