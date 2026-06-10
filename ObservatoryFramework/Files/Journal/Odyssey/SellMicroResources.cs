namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;

/// <summary>
///     Written when a player sells Microresources for cash.
/// </summary>
public class SellMicroResources : JournalBase
{
    public List<MicroResource> MicroResources { get; init; }
    public int Price { get; init; }
    public ulong MarketID { get; init; }
}