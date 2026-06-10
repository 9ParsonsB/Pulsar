namespace Observatory.Framework.Files.Journal.FleetCarrier;

/// <summary>
///     Written when fleet carrier decommissioning starts.
/// </summary>
public class CarrierDecommission : JournalBase
{
    public ulong CarrierID { get; init; }
    public long ScrapRefund { get; init; }
    public long ScrapTime { get; init; }
    public DateTimeOffset ScrapTimeUTC => DateTimeOffset.FromUnixTimeSeconds(ScrapTime);
}