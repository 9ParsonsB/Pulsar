namespace Observatory.Framework.Files.Journal.Other;

public class JoinACrew : JournalBase
{
    public override string Event => "JoinACrew";
    public string Captain { get; init; }
    public bool Telepresence { get; init; }
}