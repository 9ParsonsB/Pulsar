namespace Observatory.Framework.Files.Journal.Travel;

public class SupercruiseEntry : JournalBase
{   
    public override string Event => "SupercruiseEntry";
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }
    public bool Taxi { get; init; }
    public bool Multicrew { get; init; }
    public bool? Wanted { get; init; }
}