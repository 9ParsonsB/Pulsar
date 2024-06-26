﻿using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class MissionCompleted : JournalBase
{
    public override string Event => "MissionCompleted";
    public string Name { get; init; }
    public string LocalisedName { get; init; }
    public string Faction { get; init; }
    public ulong MissionID { get; init; }
    public string Commodity { get; init; }
    public string Commodity_Localised { get; init; }
    public int Count { get; init; }
    public string Target { get; init; }
    public string Target_Localised { get; init; }
    public string TargetType { get; init; }
    public string TargetType_Localised { get; init; }
    public long Reward { get; init; }
    [JsonConverter(typeof(StringIntConverter))]
    public int Donation { get; init; }
    public long Donated { get; init; }
    public IList<string> PermitsAwarded { get; init; }
    public List<CommodityReward> CommodityReward { get; init; }
    public List<MaterialReward> MaterialsReward { get; init; }
    public string DestinationSystem { get; init; }
    public string DestinationStation { get; init; }
    public string DestinationSettlement { get; init; }
    public string NewDestinationSystem { get; init; }
    public string NewDestinationStation { get; init; }
    public int KillCount { get; init; }
    public string TargetFaction { get; init; }
    public List<FactionEffect> FactionEffects { get; init; }
}