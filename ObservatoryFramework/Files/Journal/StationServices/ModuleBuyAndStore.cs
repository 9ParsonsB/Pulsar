namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when buying a module in outfitting and storing the replaced module.
/// </summary>
public class ModuleBuyAndStore : JournalBase
{
    public ulong MarketID { get; init; }
    public string Slot { get; init; }
    public string BuyItem { get; init; }
    public string BuyItem_Localised { get; init; }
    public uint BuyPrice { get; init; }
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
}