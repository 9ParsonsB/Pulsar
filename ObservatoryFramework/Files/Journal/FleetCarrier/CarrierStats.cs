﻿using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierStats : JournalBase
{
    public ulong CarrierID { get; init; }
    public string Callsign { get; init; }
    public string Name { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarrierDockingAccess DockingAccess { get; init; }
    public bool AllowNotorious { get; init; }
    public int FuelLevel { get; init; }
    public float JumpRangeCurr { get; init; }
    public float JumpRangeMax { get; init; }
    public bool PendingDecommission { get; init; }
    public CarrierSpaceUsage SpaceUsage { get; init; }
    public ParameterTypes.CarrierFinance Finance { get; init; }
    public ImmutableList<CarrierCrew> Crew { get; init; }
    public ImmutableList<CarrierPack> ShipPacks { get; init; }
    public ImmutableList<CarrierPack> ModulePacks { get; init; }
}