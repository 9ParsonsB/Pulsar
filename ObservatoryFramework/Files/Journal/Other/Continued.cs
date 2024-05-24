namespace Observatory.Framework.Files.Journal.Other;

public class Continued : JournalBase
{
    public override string Event => "Continued";
    public int Part { get; init; }
}