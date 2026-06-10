namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when fleet carrier materials data is updated.
/// </summary>
public class FCMaterials : JournalBase
{
    public ulong MarketID { get; init; }
    public string CarrierName { get; init; }
    public string CarrierID { get; init; }
}