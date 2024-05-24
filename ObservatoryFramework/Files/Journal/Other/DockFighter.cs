namespace Observatory.Framework.Files.Journal.Other;

public class DockFighter : JournalBase
{
    public override string Event => "DockFighter";
    public ulong ID { get; init; }
}