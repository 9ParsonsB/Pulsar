namespace Observatory.Framework.Files.Journal.Combat;

public class PVPKill : JournalBase
{
    public override string Event => "PVPKill";
    public string Victim { get; init; }
    public int CombatRank { get; init; }
}