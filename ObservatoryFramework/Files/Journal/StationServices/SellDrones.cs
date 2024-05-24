namespace Observatory.Framework.Files.Journal.StationServices;

public class SellDrones : JournalBase
{
    public override string Event => "SellDrones";
    public string Type { get; init; }
    public int Count { get; init; }
    public uint SellPrice { get; init; }
    public int TotalSale { get; init; }
}