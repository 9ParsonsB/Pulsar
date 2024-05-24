namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierJumpCancelled : JournalBase
{
    public override string Event => "CarrierJumpCancelled";
    public ulong CarrierID { get; init; }
}