namespace Observatory.Framework.Files.Journal.StationServices;

using ParameterTypes;

public class StoredShips : JournalBase
{
    public ulong MarketID { get; init; }

    /// <summary>
    ///     Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }

    public string StarSystem { get; init; }
    public List<StoredShip> ShipsHere { get; init; }
    public List<StoredShip> ShipsRemote { get; init; }
}