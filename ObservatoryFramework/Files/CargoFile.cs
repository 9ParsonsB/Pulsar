using System.Collections.Immutable;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files;

/// <summary>
/// Elite Dangerous cargo.json file. Describes the current cargo carried above the player's ship.
/// </summary>
public class CargoFile : JournalBase
{
    public override string Event => "Cargo";
    /// <summary>
    /// Type of vehicle currently being reported. "Ship" or "SRV".
    /// </summary>
    public string Vessel { get; init; }
    /// <summary>
    /// Number of different types of cargo carried(?)
    /// </summary>
    public int Count { get; init; }
    /// <summary>
    /// List of full cargo details.
    /// </summary>
    public List<CargoType> Inventory { get; init; }
}