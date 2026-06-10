namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when engaging a new member of crew.
/// </summary>
public class CrewHire : CrewFire
{
    public string Faction { get; init; }
    public long Cost { get; init; }
    public int CombatRank { get; init; }
}