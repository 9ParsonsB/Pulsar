namespace Observatory.Framework.Files.Journal.StationServices;

public class CommunityGoalDiscard : JournalBase
{
    public override string Event => "CommunityGoalDiscard";
    public ulong CGID { get; init; }
    public string Name { get; init; }
    public string System { get; init; }
}