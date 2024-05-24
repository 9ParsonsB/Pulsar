using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierCrewServices : JournalBase
{
    public override string Event => "CarrierCrewServices";
    public ulong CarrierID { get; init; }
    public string CrewRole { get; init; }
    public string CrewName { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarrierCrewOperation Operation { get; init; }
}