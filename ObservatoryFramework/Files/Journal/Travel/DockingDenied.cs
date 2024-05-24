using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Travel;

public class DockingDenied : DockingCancelled
{
    public override string Event => "DockingDenied";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Reason Reason { get; init; }
}