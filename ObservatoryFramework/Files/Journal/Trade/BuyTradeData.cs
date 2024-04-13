namespace Observatory.Framework.Files.Journal.Trade;

public class BuyTradeData : JournalBase
{
    public string System { get; init; }
    public long Cost { get; init; }
}