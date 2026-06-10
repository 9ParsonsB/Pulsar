namespace Observatory.Framework.Files.Journal.FleetCarrier;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when viewing fleet carrier stats.
/// </summary>
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
    public List<CarrierCrew> Crew { get; init; }
    public List<CarrierPack> ShipPacks { get; init; }
    public List<CarrierPack> ModulePacks { get; init; }
}