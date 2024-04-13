using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class LaunchDrone : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LimpetDrone Type { get; init; }
}