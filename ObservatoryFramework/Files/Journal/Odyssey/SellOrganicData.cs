namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;

/// <summary>
///     Written when organic data is sold to Vista Genomics.
/// </summary>
public class SellOrganicData : JournalBase
{
    public ulong MarketID { get; init; }
    public List<BioData> BioData { get; init; }
}