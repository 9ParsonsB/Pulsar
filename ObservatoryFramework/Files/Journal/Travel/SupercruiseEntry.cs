namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the player enters supercruise.
/// </summary>
public class SupercruiseEntry : JournalBase
{
    /// <summary>
    ///     Name of the current star system.
    /// </summary>
    public string StarSystem { get; init; }

    /// <summary>
    ///     Unique address of the current star system.
    /// </summary>
    public ulong SystemAddress { get; init; }

    /// <summary>
    ///     Whether the player is travelling by Apex taxi.
    /// </summary>
    public bool Taxi { get; init; }

    /// <summary>
    ///     Whether the player is in multicrew.
    /// </summary>
    public bool Multicrew { get; init; }

    /// <summary>
    ///     Whether the player is wanted locally, if reported.
    /// </summary>
    public bool? Wanted { get; init; }
}