namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the player drops from supercruise at a destination.
/// </summary>
public class SupercruiseDestinationDrop : JournalBase
{
    /// <summary>
    ///     Type of destination reached.
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    ///     Threat level of the destination, if applicable.
    /// </summary>
    public int Threat { get; init; }

    /// <summary>
    ///     Market identifier for the destination, if applicable.
    /// </summary>
    public ulong MarketID { get; init; }
}