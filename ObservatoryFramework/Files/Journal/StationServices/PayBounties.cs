namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when paying off bounties.
/// </summary>
public class PayBounties : JournalBase
{
    public long Amount { get; init; }
    public float BrokerPercentage { get; init; }
    public bool AllFines { get; init; }
    public string Faction { get; init; }
    public string Faction_Localised { get; init; }
    public ulong ShipID { get; init; }
}