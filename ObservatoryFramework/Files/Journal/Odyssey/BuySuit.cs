namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when buying a new suit.
/// </summary>
public class BuySuit : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public int Price { get; init; }
    public ulong SuitID { get; init; }
}