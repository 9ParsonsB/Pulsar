namespace Observatory.Framework.Files.Journal.StationServices;

public class CrewFire : JournalBase
{
    public override string Event => "CrewFire";
    public string Name { get; init; }
    public ulong CrewID { get; init; }
}