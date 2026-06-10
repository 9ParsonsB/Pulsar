namespace Observatory.Framework.Files.Journal.FleetCarrier;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when fleet carrier crew services are changed.
/// </summary>
public class CarrierCrewServices : JournalBase
{
    public ulong CarrierID { get; init; }
    public string CrewRole { get; init; }
    public string CrewName { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarrierCrewOperation Operation { get; init; }
}