using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Travel;

public class DockingDenied : DockingCancelled
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Reason Reason { get; init; }
}