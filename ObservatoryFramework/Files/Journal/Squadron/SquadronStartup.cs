namespace Observatory.Framework.Files.Journal.Squadron;

/// <summary>
///     Written at startup with squadron state information.
/// </summary>
public class SquadronStartup : SquadronCreated
{
    public int CurrentRank { get; init; }
}