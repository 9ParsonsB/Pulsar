namespace Observatory.Framework.Files.Journal.StationServices;

public class MissionFailed : JournalBase
{
    public override string Event => "MissionFailed";
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public ulong MissionID { get; init; }
    public long Fine { get; init; }
}