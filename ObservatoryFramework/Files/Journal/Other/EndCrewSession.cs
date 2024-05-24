namespace Observatory.Framework.Files.Journal.Other;

public class EndCrewSession : JournalBase
{
    public override string Event => "EndCrewSession";
    public bool OnCrime { get; init; }
    public bool Telepresence { get; init; }
}