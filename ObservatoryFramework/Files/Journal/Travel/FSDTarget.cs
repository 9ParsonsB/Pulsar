namespace Observatory.Framework.Files.Journal.Travel;

public class FSDTarget : JournalBase
{
    public override string Event => "FSDTarget";
    public string Name { get; init; }
    public ulong SystemAddress { get; init; }
    public string StarClass { get; init; }
    public int RemainingJumpsInRoute { get; init; }
}