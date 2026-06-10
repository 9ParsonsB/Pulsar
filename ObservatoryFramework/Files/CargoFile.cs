namespace Observatory.Framework.Files;

using Journal;
using ParameterTypes;

/// <summary>
///     Elite Dangerous cargo.json file. Describes the current cargo carried above the player's ship.
/// </summary>
public class CargoFile : JournalBase
{
    /// <summary>
    ///     Type of vehicle currently being reported. "Ship" or "SRV".
    /// </summary>
    public string Vessel { get; init; }

    /// <summary>
    ///     Number of different types of cargo carried(?)
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    ///     List of full cargo details.
    /// </summary>
    public List<CargoType> Inventory { get; init; }
}