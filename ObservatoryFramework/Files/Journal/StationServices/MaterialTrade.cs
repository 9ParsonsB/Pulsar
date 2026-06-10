namespace Observatory.Framework.Files.Journal.StationServices;

using ParameterTypes;

/// <summary>
///     Written when exchanging materials at the Material trader contact.
/// </summary>
public class MaterialTrade : JournalBase
{
    public ulong MarketID { get; init; }
    public string TraderType { get; init; }
    public TradeDetail Paid { get; init; }
    public TradeDetail Received { get; init; }
}