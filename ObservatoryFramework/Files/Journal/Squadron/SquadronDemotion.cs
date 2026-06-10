namespace Observatory.Framework.Files.Journal.Squadron;

/// <summary>
///     Written when a squadron member is demoted.
/// </summary>
public class SquadronDemotion : SquadronCreated
{
    public int OldRank { get; init; }
    public int NewRank { get; init; }
}