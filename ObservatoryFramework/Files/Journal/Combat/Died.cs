namespace Observatory.Framework.Files.Journal.Combat;

using ParameterTypes;

/// <summary>
///     Written when player was killed by a wing.
/// </summary>
public class Died : JournalBase
{
    public string KillerName { get; init; }
    public string KillerName_Localised { get; init; }
    public string KillerShip { get; init; }
    public string KillerRank { get; init; }
    public List<Killer> Killers { get; init; }
}