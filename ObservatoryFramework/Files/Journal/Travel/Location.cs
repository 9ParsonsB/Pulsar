﻿using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Travel;

public class Location : JournalBase
{
    [JsonConverter(typeof(IntBoolFlexConverter))]
    public bool Docked { get; init; }
    public double DistFromStarLS { get; init; }
    
    /// <summary>
    /// Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }
    public string StationType { get; init; }
    public float Longitude { get; init; }
    public float Latitude { get; init; }
    public ulong MarketID { get; init; }

    [JsonConverter(typeof(LegacyFactionConverter<Faction>))]
    public Faction StationFaction { get; init; }
    public string StationGovernment { get; init; }
    public string StationGovernment_Localised { get; init; }
    public string StationAllegiance { get; init; }
    public ImmutableList<string> StationServices { get; init; }
    public string StationEconomy { get; init; }
    public string StationEconomy_Localised { get; init; }
    public ImmutableList<StationEconomy> StationEconomies { get; init; }
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }

    [JsonConverter(typeof(StarPosConverter))]
    public (double x, double y, double z) StarPos { get; init; }
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
    public string Body { get; init; }
    public int BodyID { get; init; }
    public string BodyType { get; init; }
    public ImmutableList<DetailedFaction> Factions { get; init; }

    [JsonConverter(typeof(LegacyFactionConverter<DetailedFaction>))]
    public DetailedFaction SystemFaction { get; init; }
    public ImmutableList<Conflict> Conflicts { get; init; }
    public ImmutableList<string> Powers { get; init; }
    public string PowerplayState { get; init; }
    public bool Taxi { get; init; }
    public bool Multicrew { get; init; }
    public bool OnFoot { get; init; }
    public bool InSRV { get; init; }
    public ThargoidWar ThargoidWar { get; init; }
}