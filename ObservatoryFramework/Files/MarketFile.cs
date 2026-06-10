namespace Observatory.Framework.Files;

using Journal;
using ParameterTypes;

/// <summary>
///     Elite Dangerous market.json file. Contains details about all available commodities available at the current
///     station.
/// </summary>
public class MarketFile : JournalBase
{
    /// <summary>
    ///     Unique ID of current market.
    /// </summary>
    public ulong MarketID { get; init; }

    /// <summary>
    ///     Name of the station where the market is located.
    /// </summary>
    public string StationName { get; init; }

    /// <summary>
    ///     Type of station where the market is located.
    /// </summary>
    public string StationType { get; init; }

    /// <summary>
    ///     Name of star system where the market is located.
    /// </summary>
    public string StarSystem { get; init; }

    /// <summary>
    ///     List of all commodities available in the market.
    /// </summary>
    public List<MarketItem> Items { get; init; }
}