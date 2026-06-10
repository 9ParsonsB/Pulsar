namespace Observatory.Framework.Files.Journal.Trade;

/// <summary>
///     Written when purchasing goods in the market.
/// </summary>
public class MarketBuy : JournalBase
{
    public ulong MarketID { get; init; }
    public string Type { get; init; }
    public string Type_Localised { get; init; }
    public int Count { get; init; }
    public int BuyPrice { get; init; }
    public long TotalCost { get; init; }
}