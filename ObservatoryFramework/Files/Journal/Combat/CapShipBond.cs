namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when The player has been rewarded for a capital ship combat.
/// </summary>
public class CapShipBond : JournalBase
{
    public long Reward { get; init; }
    public string AwardingFaction { get; init; }
    public string VictimFaction { get; init; }
}