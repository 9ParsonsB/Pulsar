namespace Observatory.Framework.Files.Journal.Squadron;

public class SquadronStartup : SquadronCreated
{
    public override string Event => "SquadronStartup";
    public int CurrentRank { get; init; }
}