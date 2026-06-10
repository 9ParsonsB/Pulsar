namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when purchasing drones.
/// </summary>
public class BuyDrones : JournalBase
{
    public string Type { get; init; }
    public int Count { get; init; }
    public uint BuyPrice { get; init; }
    public uint SellPrice { get; init; }
    public int TotalCost { get; init; }
}