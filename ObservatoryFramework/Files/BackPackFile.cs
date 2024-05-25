using System.Collections.Immutable;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files;

/// <summary>
/// Elite Dangerous backpack.json file. Describes all the items currently carried by the player.
/// </summary>
public class BackpackFile : JournalBase
{
    public override string Event => "Backpack";
    /// <summary>
    /// List of all items carried.
    /// </summary>
    public IReadOnlyCollection<BackpackItem> Items { get; init; }
    /// <summary>
    /// List of all components carried.
    /// </summary>
    public IReadOnlyCollection<BackpackItem> Components { get; init; }
    /// <summary>
    /// List of player consumable items carried.
    /// </summary>
    public IReadOnlyCollection<BackpackItem> Consumables { get; init; }
    /// <summary>
    /// List of all data currently stored by the player.
    /// </summary>
    public IReadOnlyCollection<BackpackItem> Data { get; init; }
}