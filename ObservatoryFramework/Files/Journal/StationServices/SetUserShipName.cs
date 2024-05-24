namespace Observatory.Framework.Files.Journal.StationServices;

public class SetUserShipName : JournalBase
{
    public override string Event => "SetUserShipName";
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
    public string UserShipName { get; init; }
    public string UserShipId { get; init; }
}