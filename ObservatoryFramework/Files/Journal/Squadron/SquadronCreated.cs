namespace Observatory.Framework.Files.Journal.Squadron;

public class SquadronCreated : JournalBase
{
    public override string Event => "SquadronCreated";
    public string SquadronName { get; init; }
}