namespace Observatory.Framework.Files.Journal.Squadron;

public class SquadronDemotion : SquadronCreated
{
    public override string Event => "SquadronDemotion";
    public int OldRank { get; init; }
    public int NewRank { get; init; }
}