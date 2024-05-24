namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierDecommission : JournalBase
{
    public override string Event => "CarrierDecommission";
    public ulong CarrierID { get; init; }
    public long ScrapRefund { get; init; }
    public long ScrapTime { get; init; }
    public DateTimeOffset ScrapTimeUTC => DateTimeOffset.FromUnixTimeSeconds(ScrapTime);
}