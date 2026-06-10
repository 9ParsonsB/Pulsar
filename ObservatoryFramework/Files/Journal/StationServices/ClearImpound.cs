namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when When clearing the impound off of one of your ships.
/// </summary>
public class ClearImpound : JournalBase
{
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public ulong ShipID { get; init; }
    public ulong ShipMarketID { get; init; }
    public ulong MarketID { get; init; }
}