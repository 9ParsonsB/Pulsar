namespace Observatory.Framework.Files.Journal.FleetCarrier;

using Travel;

/// <summary>
///     Written when fleet carrier materials data is updated.
/// </summary>
public class FCMaterials : FSDJump
{
    public ulong MarketID { get; init; }
    public string CarrierName { get; init; }
    public ulong CarrierID { get; init; }
}