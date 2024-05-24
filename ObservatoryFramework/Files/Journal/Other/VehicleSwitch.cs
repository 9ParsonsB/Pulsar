using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class VehicleSwitch : JournalBase
{
    public override string Event => "VehicleSwitch";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VehicleSwitchTo To { get; init; }
}