namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the player lands on a planetary or station surface.
/// </summary>
public class Touchdown : JournalBase
{
    /// <summary>
    ///     Latitude of the landing site.
    /// </summary>
    public double Latitude { get; init; }

    /// <summary>
    ///     Longitude of the landing site.
    /// </summary>
    public double Longitude { get; init; }

    /// <summary>
    ///     Nearest destination to the landing site, if one is known.
    /// </summary>
    public string? NearestDestination { get; init; }

    /// <summary>
    ///     Localized form of <see cref="NearestDestination" />, if available.
    /// </summary>
    public string? NearestDestination_Localised { get; init; }

    /// <summary>
    ///     Whether the landing was performed by the player rather than automation or transit.
    /// </summary>
    public bool PlayerControlled { get; init; }

    /// <summary>
    ///     Whether the player is travelling by Apex taxi.
    /// </summary>
    public bool Taxi { get; init; }

    /// <summary>
    ///     Whether the player is in multicrew.
    /// </summary>
    public bool Multicrew { get; init; }

    /// <summary>
    ///     Name of the current star system.
    /// </summary>
    public string StarSystem { get; init; }

    /// <summary>
    ///     Unique address of the current star system.
    /// </summary>
    public ulong SystemAddress { get; init; }

    /// <summary>
    ///     Name of the body where touchdown occurred.
    /// </summary>
    public string Body { get; init; }

    /// <summary>
    ///     Numeric identifier of the body where touchdown occurred.
    /// </summary>
    public int BodyID { get; init; }

    /// <summary>
    ///     Whether touchdown occurred on a station surface.
    /// </summary>
    public bool OnStation { get; init; }

    /// <summary>
    ///     Whether touchdown occurred on a planetary surface.
    /// </summary>
    public bool OnPlanet { get; init; }
}