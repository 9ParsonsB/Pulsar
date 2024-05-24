namespace Observatory.Framework.Files.Journal.Travel;

public class SupercruiseDestinationDrop : JournalBase
{
    public override string Event => "SupercruiseDestinationDrop";
    public string Type { get; init; }
    public int Threat { get; init; }
    public ulong MarketID { get; init; }
}