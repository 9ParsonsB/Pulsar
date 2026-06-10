namespace Observatory.Framework.Files.Journal.Travel;

using Converters;
using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when the player jumps from one star system to another.
/// </summary>
public class FSDJump : JournalBase
{
    /// <summary>
    ///     Name of the destination star system.
    /// </summary>
    public string StarSystem { get; init; }

    /// <summary>
    ///     Unique address of the destination star system.
    /// </summary>
    public ulong SystemAddress { get; init; }

    /// <summary>
    ///     Position of the destination star system in light-years as X, Y, and Z coordinates.
    /// </summary>
    [JsonConverter(typeof(StarPosConverter))]
    public StarPos StarPos { get; init; }

    /// <summary>
    ///     Name of the arrival body in the destination system.
    /// </summary>
    public string Body { get; init; }

    /// <summary>
    ///     Numeric identifier of the arrival body within the destination system.
    /// </summary>
    public int BodyID { get; init; }

    /// <summary>
    ///     Type of the arrival body.
    /// </summary>
    public string BodyType { get; init; }

    /// <summary>
    ///     Distance jumped, in light-years.
    /// </summary>
    public double JumpDist { get; init; }

    /// <summary>
    ///     Amount of fuel consumed by the jump.
    /// </summary>
    public double FuelUsed { get; init; }

    /// <summary>
    ///     Fuel remaining after the jump.
    /// </summary>
    public double FuelLevel { get; init; }

    /// <summary>
    ///     Indicates whether an FSD boost was used for the jump.
    /// </summary>
    public int BoostUsed { get; init; }

    /// <summary>
    ///     Controlling faction of the destination system.
    /// </summary>
    [JsonConverter(typeof(LegacyFactionConverter<Faction>))]
    public Faction? SystemFaction { get; init; }

    /// <summary>
    ///     Superpower allegiance of the destination system.
    /// </summary>
    public string SystemAllegiance { get; init; }

    /// <summary>
    ///     Internal identifier for the destination system's primary economy.
    /// </summary>
    public string SystemEconomy { get; init; }

    /// <summary>
    ///     Localized form of <see cref="SystemEconomy" />.
    /// </summary>
    public string SystemEconomy_Localised { get; init; }

    /// <summary>
    ///     Internal identifier for the destination system's secondary economy.
    /// </summary>
    public string SystemSecondEconomy { get; init; }

    /// <summary>
    ///     Localized form of <see cref="SystemSecondEconomy" />.
    /// </summary>
    public string SystemSecondEconomy_Localised { get; init; }

    /// <summary>
    ///     Internal identifier for the destination system government.
    /// </summary>
    public string SystemGovernment { get; init; }

    /// <summary>
    ///     Localized form of <see cref="SystemGovernment" />.
    /// </summary>
    public string SystemGovernment_Localised { get; init; }

    /// <summary>
    ///     Internal identifier for the destination system security state.
    /// </summary>
    public string SystemSecurity { get; init; }

    /// <summary>
    ///     Localized form of <see cref="SystemSecurity" />.
    /// </summary>
    public string SystemSecurity_Localised { get; init; }

    /// <summary>
    ///     Population of the destination system.
    /// </summary>
    public long Population { get; init; }

    /// <summary>
    ///     Whether the player is wanted in the destination system.
    /// </summary>
    public bool Wanted { get; init; }

    /// <summary>
    ///     Minor factions present in the destination system.
    /// </summary>
    public List<Faction> Factions { get; init; }

    /// <summary>
    ///     Active conflicts in the destination system, if any.
    /// </summary>
    public List<Conflict> Conflicts { get; init; }

    /// <summary>
    ///     Powers contesting or controlling the destination system in Powerplay.
    /// </summary>
    public List<string> Powers { get; init; }

    /// <summary>
    ///     Current Powerplay state of the destination system.
    /// </summary>
    public string? PowerplayState { get; init; }

    /// <summary>
    ///     Whether the player arrived via Apex taxi.
    /// </summary>
    public bool Taxi { get; init; }

    /// <summary>
    ///     Whether the player was in multicrew when the jump completed.
    /// </summary>
    public bool Multicrew { get; init; }

    /// <summary>
    ///     Current Thargoid war state for the destination system, if applicable.
    /// </summary>
    public ThargoidWar? ThargoidWar { get; init; }
}