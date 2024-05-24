using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class LaunchDrone : JournalBase
{
    public override string Event => "LaunchDrone";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LimpetDrone Type { get; init; }
}