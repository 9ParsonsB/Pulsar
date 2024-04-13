namespace Observatory.Framework.Files.Journal.StationServices;

public class CrewHire : CrewFire
{
    public string Faction { get; init; }
    public long Cost { get; init; }
    public int CombatRank { get; init; }
}