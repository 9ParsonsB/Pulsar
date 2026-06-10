namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when opting out of a community goal.
/// </summary>
public class CommunityGoalDiscard : JournalBase
{
    public ulong CGID { get; init; }
    public string Name { get; init; }
    public string System { get; init; }
}