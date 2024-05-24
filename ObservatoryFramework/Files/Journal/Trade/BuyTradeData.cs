namespace Observatory.Framework.Files.Journal.Trade;

public class BuyTradeData : JournalBase
{
    public override string Event => "BuyTradeData";
    public string System { get; init; }
    public long Cost { get; init; }
}