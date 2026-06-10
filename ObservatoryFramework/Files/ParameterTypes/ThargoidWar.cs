namespace Observatory.Framework.Files.ParameterTypes;

using Converters;
using System.Text.Json.Serialization;

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