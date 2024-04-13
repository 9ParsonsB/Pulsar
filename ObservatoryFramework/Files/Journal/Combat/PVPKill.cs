namespace Observatory.Framework.Files.Journal.Combat;

public class PVPKill : JournalBase
{
    public string Victim { get; init; }
    public int CombatRank { get; init; }
}