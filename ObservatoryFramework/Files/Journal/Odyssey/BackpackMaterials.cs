namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;

/// <summary>
///     Written at startup or when the backpack inventory changes.
/// </summary>
public class BackpackMaterials : JournalBase
{
    public List<BackpackItem> Items { get; init; }
    public List<BackpackItem> Components { get; init; }
    public List<BackpackItem> Consumables { get; init; }
    public List<BackpackItem> Data { get; init; }
}