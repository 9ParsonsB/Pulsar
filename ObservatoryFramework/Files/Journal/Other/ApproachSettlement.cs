﻿using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class ApproachSettlement : JournalBase
{
    public override string Event => "ApproachSettlement";
    public ulong SystemAddress { get; init; }
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public ulong MarketID { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
    public int BodyID { get; init; }
    public string BodyName { get; init; }
    public ImmutableList<StationEconomy> StationEconomies { get; init; }
    public string StationEconomy { get; init; }
    public string StationEconomy_Localised { get; init; }
    public Faction StationFaction { get; init; }
    public string StationGovernment { get; init; }
    public string StationGovernment_Localised { get; init; }
    [JsonConverter(typeof(StationServiceConverter))]
    public StationService StationServices { get; init; }
}