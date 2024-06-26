﻿using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;
using Observatory.Framework.Files.Journal.Travel;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierJump : FSDJump
{
    public override string Event => "CarrierJump";
    public bool Docked { get; init; }
    public bool OnFoot { get; init; }
    /// <summary>
    /// Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }
    public string StationType { get; init; }
    public ulong MarketID { get; init; }
    public Faction StationFaction { get; init; }
    public string StationGovernment { get; init; }
    public string StationGovernment_Localised { get; init; }
    [JsonConverter(typeof(StationServiceConverter))]
    public StationService StationServices { get; init; }
    public string StationEconomy { get; init; }
    public string StationEconomy_Localised { get; init; }
    public List<StationEconomy> StationEconomies { get; init; }
}