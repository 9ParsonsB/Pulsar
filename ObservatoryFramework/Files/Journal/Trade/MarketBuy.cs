﻿namespace Observatory.Framework.Files.Journal.Trade;

public class MarketBuy : JournalBase
{
    public override string Event => "MarketBuy";
    public ulong MarketID { get; init; }
    public string Type { get; init; }
    public string Type_Localised { get; init; }
    public int Count { get; init; }
    public int BuyPrice { get; init; } 
    public long TotalCost { get; init; }
}