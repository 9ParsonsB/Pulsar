namespace Observatory.Framework.Files.Journal.Powerplay;

/// <summary>
///     Written when collecting powerplay commodities for delivery.
/// </summary>
public class PowerplayCollect : PowerplayJoin
{
    public string Type { get; init; }
    public string Type_Localised { get; init; }
    public int Count { get; init; }
}