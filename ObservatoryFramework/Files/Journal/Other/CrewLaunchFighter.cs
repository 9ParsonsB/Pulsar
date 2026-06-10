namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when in multicrew, in Helm player's log, when a crew member launches a fighter.
/// </summary>
public class CrewLaunchFighter : CrewMemberJoins
{
    public ulong ID { get; init; }
}