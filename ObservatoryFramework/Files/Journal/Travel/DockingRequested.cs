namespace Observatory.Framework.Files.Journal.Travel;

using ParameterTypes;

/// <summary>
///     Written when the player requests docking at a station.
/// </summary>
public class DockingRequested : JournalBase
{
    /// <summary>
    ///     Name of the station receiving the docking request.
    /// </summary>
    public string StationName { get; init; }

    /// <summary>
    ///     Type of station receiving the docking request.
    /// </summary>
    public string StationType { get; init; }

    /// <summary>
    ///     Unique market identifier for the station.
    /// </summary>
    public ulong MarketID { get; init; }

    /// <summary>
    ///     Availability of small, medium, and large landing pads at the station.
    /// </summary>
    public LandingPads LandingPads { get; init; }
}