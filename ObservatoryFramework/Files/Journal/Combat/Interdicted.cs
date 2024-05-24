namespace Observatory.Framework.Files.Journal.Combat;

public class Interdicted : JournalBase
{
    public override string Event => "Interdicted";
    public bool Submitted { get; init; }
    public string Interdictor { get; init; }
    public string Interdictor_Localised { get; init; }
    public bool IsPlayer { get; init; }
    public int CombatRank { get; init; }
    public string Faction { get; init; }
    public string Power { get; init; }
    public bool IsThargoid { get; init; }
}