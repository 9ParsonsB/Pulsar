namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when this player has killed another player.
/// </summary>
public class PVPKill : JournalBase
{
    public string Victim { get; init; }
    public int CombatRank { get; init; }
}