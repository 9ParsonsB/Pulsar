using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class VehicleSwitch : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VehicleSwitchTo To { get; init; }
}