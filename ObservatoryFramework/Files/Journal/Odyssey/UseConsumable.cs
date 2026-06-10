namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when an on-foot consumable is used.
/// </summary>
public class UseConsumable : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public string Type { get; init; }
}