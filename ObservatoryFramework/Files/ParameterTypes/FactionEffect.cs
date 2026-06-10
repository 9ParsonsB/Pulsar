namespace Observatory.Framework.Files.ParameterTypes;

using Converters;
using System.Text.Json.Serialization;

public class FactionEffect
{
    public string Faction { get; init; }
    public List<EffectType> Effects { get; init; }
    public List<InfluenceType> Influence { get; init; }

    [JsonConverter(typeof(RepInfConverter))]
    public int Reputation { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrendValue ReputationTrend { get; init; }
}