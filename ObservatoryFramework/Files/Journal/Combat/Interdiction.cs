namespace Observatory.Framework.Files.Journal.Combat;

public class Interdiction : JournalBase
{
    public override string Event => "Interdiction";
    public bool Success { get; init; }
    public string Interdictor { get; init; }
    public bool IsPlayer { get; init; }
    public int CombatRank { get; init; }
    public string Faction { get; init; }
    public string Power { get; init; }
}