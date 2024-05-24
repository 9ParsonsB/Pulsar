namespace Observatory.Framework.Files.Journal.StationServices;

public class MissionAbandoned : JournalBase
{
    public override string Event => "MissionAbandoned";
    public string Name { get; init; }
    public ulong MissionID { get; init; }
    public long Fine { get; init; }
}