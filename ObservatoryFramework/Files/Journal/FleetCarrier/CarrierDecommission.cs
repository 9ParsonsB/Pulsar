namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierDecommission : JournalBase
{
    public ulong CarrierID { get; init; }
    public long ScrapRefund { get; init; }
    public long ScrapTime { get; init; }
    public DateTime ScrapTimeUTC => DateTimeOffset.FromUnixTimeSeconds(ScrapTime).UtcDateTime;
}