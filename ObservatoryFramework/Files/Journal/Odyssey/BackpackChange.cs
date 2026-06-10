namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;

/// <summary>
///     Written when the backpack inventory changes.
/// </summary>
public class BackpackChange : JournalBase
{
    public List<BackpackItemChange> Added { get; init; }
    public List<BackpackItemChange> Removed { get; init; }
}