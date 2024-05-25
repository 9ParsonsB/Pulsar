namespace Observatory.Framework.Files.Journal.StationServices;

public class ModuleBuyAndStore : JournalBase
{
    public override string Event => "ModuleBuyAndStore";
    public ulong MarketID { get; init; }
    public string Slot { get; init; }
    public string BuyItem { get; init; }
    public string BuyItem_Localised { get; init; }
    public uint BuyPrice { get; init; }
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
}