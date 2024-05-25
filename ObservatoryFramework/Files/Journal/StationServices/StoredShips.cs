using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class StoredShips : JournalBase
{
    public override string Event => "StoredShips";
    public ulong MarketID { get; init; }
    /// <summary>
    /// Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }
    public string StarSystem { get; init; }
    public IReadOnlyCollection<StoredShip> ShipsHere { get; init; }
    public IReadOnlyCollection<StoredShip> ShipsRemote { get; init; }
}