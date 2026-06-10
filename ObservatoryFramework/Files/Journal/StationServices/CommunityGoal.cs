namespace Observatory.Framework.Files.Journal.StationServices;

using ParameterTypes;

/// <summary>
///     Written when the game retrieves info on community goals from the server, and the data has changed since last time.
/// </summary>
public class CommunityGoal : JournalBase
{
    public List<CurrentGoal> CurrentGoals { get; init; }
}