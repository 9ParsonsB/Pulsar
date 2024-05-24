namespace Observatory.Framework.Files.Journal.Other;

public class Resurrect : JournalBase
{
    public override string Event => "Resurrect";
    public string Option { get; init; }
    public int Cost { get; init; }
    public bool Bankrupt { get; init; }
}