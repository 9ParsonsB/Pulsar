namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierCancelDecommission : JournalBase
{
    public override string Event => "CarrierCancelDecommission";
    public ulong CarrierID { get; init; }
}