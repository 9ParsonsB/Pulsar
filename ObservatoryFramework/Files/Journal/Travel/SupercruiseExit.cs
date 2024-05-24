namespace Observatory.Framework.Files.Journal.Travel;

public class SupercruiseExit : SupercruiseEntry
{
    public override string Event => "SupercruiseExit";
    public string Body { get; init; }
    public int BodyID { get; init; }
    public string BodyType { get; init; }
}