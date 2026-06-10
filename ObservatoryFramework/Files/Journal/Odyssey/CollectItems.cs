namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when on-foot items are collected.
/// </summary>
public class CollectItems : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public string Type { get; init; }
    public ulong OwnerID { get; init; }
    public int Count { get; init; }
    public bool Stolen { get; init; }
}