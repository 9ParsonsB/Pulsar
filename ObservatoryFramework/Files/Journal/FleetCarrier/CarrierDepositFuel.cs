namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierDepositFuel : JournalBase
{
    public override string Event => "CarrierDepositFuel";
    public ulong CarrierID { get; init; }
    public int Amount { get; init; }
    public int Total { get; init; }
}