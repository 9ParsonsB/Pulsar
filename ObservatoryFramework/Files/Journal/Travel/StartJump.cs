namespace Observatory.Framework.Files.Journal.Travel;

public class StartJump : JournalBase
{
    public override string Event => "StartJump";
    public string JumpType { get; init; }
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }
    public string StarClass { get; init; }
    public bool Taxi {  get; init; }
}