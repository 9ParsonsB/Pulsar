namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when selling unwanted drones back to the market.
/// </summary>
public class SellDrones : JournalBase
{
    public string Type { get; init; }
    public int Count { get; init; }
    public uint SellPrice { get; init; }
    public int TotalSale { get; init; }
}