namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when player has (attempted to) interdict another player or npc.
/// </summary>
public class Interdiction : JournalBase
{
    public bool Success { get; init; }
    public string Interdictor { get; init; }
    public bool IsPlayer { get; init; }
    public int CombatRank { get; init; }
    public string Faction { get; init; }
    public string Power { get; init; }
}