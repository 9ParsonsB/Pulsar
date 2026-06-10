namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the player undocks from a station.
/// </summary>
public class Undocked : JournalBase
{
    /// <summary>
    ///     Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }

    /// <summary>
    ///     Type of station the player undocked from.
    /// </summary>
    public string StationType { get; init; }

    /// <summary>
    ///     Unique market identifier for the station.
    /// </summary>
    public ulong MarketID { get; init; }

    /// <summary>
    ///     Whether the player is travelling by Apex taxi.
    /// </summary>
    public bool Taxi { get; init; }

    /// <summary>
    ///     Whether the player is in multicrew.
    /// </summary>
    public bool Multicrew { get; init; }
}