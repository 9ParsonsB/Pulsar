namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierJumpRequest : JournalBase
{
    public override string Event => "CarrierJumpRequest";
    public string Body { get; init; }
    public int BodyID { get; init; }
    public ulong SystemAddress { get; init; }
    public ulong CarrierID { get; init; }
    public string SystemName { get; init; }
    public ulong SystemID { get; init; }
    public DateTimeOffset DepartureTime { get; init; }
}