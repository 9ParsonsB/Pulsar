namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when Player rewarded for taking part in a combat zone.
/// </summary>
public class FactionKillBond : JournalBase
{
    public long Reward { get; init; }
    public string AwardingFaction { get; init; }
    public string AwardingFaction_Localised { get; init; }
    public string VictimFaction { get; init; }
    public string VictimFaction_Localised { get; init; }
}