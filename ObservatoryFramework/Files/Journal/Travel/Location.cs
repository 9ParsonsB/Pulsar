namespace Observatory.Framework.Files.Journal.Travel;

using Converters;
using ParameterTypes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/// <summary>
///     Written to describe the player's current location after loading into the game or entering a new context.
/// </summary>
public class Location : JournalBase
{
    /// <summary>
    ///     Whether the player is currently docked.
    /// </summary>
    [JsonConverter(typeof(IntBoolFlexConverter))]
    public bool Docked { get; init; }

    /// <summary>
    ///     Distance from the main star to the current location, in light-seconds.
    /// </summary>
    public double? DistFromStarLS { get; init; }

    /// <summary>
    ///     Name of the station at which this event occurred.
    /// </summary>
    public string? StationName { get; init; }

    /// <summary>
    ///     Type of station, if the player is at a station.
    /// </summary>
    public string? StationType { get; init; }

    /// <summary>
    ///     Longitude on the current planetary surface, if applicable.
    /// </summary>
    public float? Longitude { get; init; }

    /// <summary>
    ///     Latitude on the current planetary surface, if applicable.
    /// </summary>
    public float? Latitude { get; init; }

    /// <summary>
    ///     Unique market identifier for the current station, if applicable.
    /// </summary>
    public ulong? MarketID { get; init; }

    /// <summary>
    ///     Faction controlling the current station, if applicable.
    /// </summary>
    [JsonConverter(typeof(LegacyFactionConverter<Faction>))]
    public Faction? StationFaction { get; init; }

    /// <summary>
    ///     Government type of the current station controller, if applicable.
    /// </summary>
    public string? StationGovernment { get; init; }

    /// <summary>
    ///     Localized form of <see cref="StationGovernment" />, if applicable.
    /// </summary>
    public string? StationGovernment_Localised { get; init; }

    /// <summary>
    ///     Superpower allegiance of the current station, if applicable.
    /// </summary>
    public string? StationAllegiance { get; init; }

    /// <summary>
    ///     Services available at the current station, if applicable.
    /// </summary>
    public List<string>? StationServices { get; init; }

    /// <summary>
    ///     Internal identifier for the current station's primary economy, if applicable.
    /// </summary>
    public string? StationEconomy { get; init; }

    /// <summary>
    ///     Localized form of <see cref="StationEconomy" />, if applicable.
    /// </summary>
    public string? StationEconomy_Localised { get; init; }

    /// <summary>
    ///     All station economies with their proportions, if applicable.
    /// </summary>
    public List<StationEconomy> StationEconomies { get; init; }

    /// <summary>
    ///     Name of the current star system.
    /// </summary>
    public string StarSystem { get; init; }

    /// <summary>
    ///     Unique address of the current star system.
    /// </summary>
    public ulong? SystemAddress { get; init; }

    /// <summary>
    ///     Position of the current star system in light-years as X, Y, and Z coordinates.
    /// </summary>
    [JsonConverter(typeof(StarPosConverter))]
    public StarPos StarPos { get; init; }

    /// <summary>
    ///     Superpower allegiance of the current star system.
    /// </summary>
    public string SystemAllegiance { get; init; }

    /// <summary>
    ///     Internal identifier for the current system's primary economy.
    /// </summary>
    public string SystemEconomy { get; init; }

    /// <summary>
    ///     Localized form of <see cref="SystemEconomy" />.
    /// </summary>
    public string SystemEconomy_Localised { get; init; }

    /// <summary>
    ///     Internal identifier for the current system's secondary economy, if present.
    /// </summary>
    public string? SystemSecondEconomy { get; init; }

    /// <summary>
    ///     Localized form of <see cref="SystemSecondEconomy" />, if present.
    /// </summary>
    public string? SystemSecondEconomy_Localised { get; init; }

    /// <summary>
    ///     Internal identifier for the current system government.
    /// </summary>
    public string SystemGovernment { get; init; }

    /// <summary>
    ///     Localized form of <see cref="SystemGovernment" />.
    /// </summary>
    public string SystemGovernment_Localised { get; init; }

    /// <summary>
    ///     Internal identifier for the current system security state.
    /// </summary>
    public string SystemSecurity { get; init; }

    /// <summary>
    ///     Localized form of <see cref="SystemSecurity" />.
    /// </summary>
    public string SystemSecurity_Localised { get; init; }

    /// <summary>
    ///     Population of the current star system, if known.
    /// </summary>
    public long? Population { get; init; }

    /// <summary>
    ///     Name of the current body.
    /// </summary>
    public string Body { get; init; }

    /// <summary>
    ///     Numeric identifier of the current body within the system.
    /// </summary>
    public int? BodyID { get; init; }

    /// <summary>
    ///     Type of the current body.
    /// </summary>
    public string BodyType { get; init; }

    /// <summary>
    ///     Minor factions present in the current system.
    /// </summary>
    public List<Faction>? Factions { get; init; }

    /// <summary>
    ///     Controlling faction of the current system.
    /// </summary>
    [JsonConverter(typeof(LegacyFactionConverter<Faction>))]
    [NotMapped]
    public Faction? SystemFaction { get; init; }

    /// <summary>
    ///     Active conflicts in the current system, if any.
    /// </summary>
    public List<Conflict>? Conflicts { get; init; }

    /// <summary>
    ///     Powers contesting or controlling the current system in Powerplay, if any.
    /// </summary>
    public List<string>? Powers { get; init; }

    /// <summary>
    ///     Current Powerplay state of the system, if applicable.
    /// </summary>
    public string? PowerplayState { get; init; }

    /// <summary>
    ///     Whether the player is travelling by Apex taxi.
    /// </summary>
    public bool? Taxi { get; init; }

    /// <summary>
    ///     Whether the player is in multicrew.
    /// </summary>
    public bool? Multicrew { get; init; }

    /// <summary>
    ///     Whether the player is currently on foot.
    /// </summary>
    public bool? OnFoot { get; init; }

    /// <summary>
    ///     Whether the player is currently in an SRV.
    /// </summary>
    public bool? InSRV { get; init; }

    /// <summary>
    ///     Current Thargoid war state for the system, if applicable.
    /// </summary>
    public ThargoidWar? ThargoidWar { get; init; }
}

/// <summary>
///     Cartesian coordinates of a star system in light-years.
/// </summary>
public class StarPos
{
    /// <summary>
    ///     X coordinate.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    ///     Y coordinate.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    ///     Z coordinate.
    /// </summary>
    public double Z { get; set; }
}