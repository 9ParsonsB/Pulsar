using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;

namespace Observatory.Framework.Files.ParameterTypes;

public class ThargoidWar
{
    public string CurrentState { get; init; }
    public string NextStateSuccess { get; init; }
    public string NextStateFailure { get; init; }
    public bool SuccessStateReached { get; init; }
    public double WarProgress { get; init; }
    public int RemainingPorts { get; init; }
    [JsonConverter(typeof(ThargoidWarRemainingTimeConverter))]
    public int EstimatedRemainingTime { get; init; }
}