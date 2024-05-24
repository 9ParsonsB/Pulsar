namespace Observatory.Framework.Files.Journal.StationServices;

public class BuyDrones : JournalBase
{
    public override string Event => "BuyDrones";
    public string Type { get; init; }
    public int Count { get; init; }
    public uint BuyPrice { get; init; }
    public uint SellPrice { get; init; }
    public int TotalCost { get; init; }
}