namespace Observatory.Framework.Files;

using Journal;
using ParameterTypes;

/// <summary>
///     Elite Dangerous backpack.json file. Describes all the items currently carried by the player.
/// </summary>
public class BackpackFile : JournalBase
{
    /// <summary>
    ///     List of all items carried.
    /// </summary>
    public List<BackpackItem> Items { get; init; }

    /// <summary>
    ///     List of all components carried.
    /// </summary>
    public List<BackpackItem> Components { get; init; }

    /// <summary>
    ///     List of player consumable items carried.
    /// </summary>
    public List<BackpackItem> Consumables { get; init; }

    /// <summary>
    ///     List of all data currently stored by the player.
    /// </summary>
    public List<BackpackItem> Data { get; init; }
}