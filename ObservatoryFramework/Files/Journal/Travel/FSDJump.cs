using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Travel;

public class FSDJump : JournalBase
{
    public override string Event => "FSDJump";
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }
    [JsonConverter(typeof(StarPosConverter))]
    public StarPos StarPos { get; init; }
    public string Body { get; init; }
    public int BodyID { get; init; }
    public string BodyType { get; init; }
    public double JumpDist { get; init; }
    public double FuelUsed { get; init; }
    public double FuelLevel { get; init; }
    public int BoostUsed { get; init; }
    [JsonConverter(typeof(LegacyFactionConverter<SystemFaction>))]
    public SystemFaction SystemFaction { get; init; }

    public string SystemAllegiance { get; init; }
    public string SystemEconomy { get; init; }
    public string SystemEconomy_Localised { get; init; }
    public string SystemSecondEconomy { get; init; }
    public string SystemSecondEconomy_Localised { get; init; }
    public string SystemGovernment { get; init; }
    public string SystemGovernment_Localised { get; init; }
    public string SystemSecurity { get; init; }
    public string SystemSecurity_Localised { get; init; }
    public long Population { get; init; }
    public bool Wanted { get; init; }
    public IReadOnlyCollection<SystemFaction> Factions { get; init; }
    public IReadOnlyCollection<Conflict> Conflicts { get; init; }
    public ICollection<string> Powers { get; init; }
    public string PowerplayState { get; init; }
    public bool Taxi { get; init; }
    public bool Multicrew { get; init; }
    public ThargoidWar ThargoidWar { get; init; }
}