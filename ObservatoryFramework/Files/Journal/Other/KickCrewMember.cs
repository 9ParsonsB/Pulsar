namespace Observatory.Framework.Files.Journal.Other;

public class KickCrewMember : JournalBase
{
    public override string Event => "KickCrewMember";
    public string Crew { get; init; }
    public bool OnCrime { get; init; }
    public bool Telepresence { get; init; }
}