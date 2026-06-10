namespace Observatory.Framework.Files.Journal.Trade;

/// <summary>
///     Written when buying trade data in the galaxy map.
/// </summary>
public class BuyTradeData : JournalBase
{
    public string System { get; init; }
    public long Cost { get; init; }
}