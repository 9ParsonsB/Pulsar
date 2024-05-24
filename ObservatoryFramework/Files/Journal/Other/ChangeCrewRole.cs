namespace Observatory.Framework.Files.Journal.Other;

public class ChangeCrewRole : JournalBase
{
    public override string Event => "ChangeCrewRole";
    public string Role { get; init; }
    public bool Telepresence { get; init; }
}