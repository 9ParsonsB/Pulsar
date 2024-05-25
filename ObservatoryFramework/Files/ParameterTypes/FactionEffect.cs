using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;

namespace Observatory.Framework.Files.ParameterTypes;

public class FactionEffect
{
    public string Faction { get; init; }
    public IReadOnlyCollection<EffectType> Effects { get; init; }
    public IReadOnlyCollection<InfluenceType> Influence { get; init; }
    [JsonConverter(typeof(RepInfConverter))]
    public int Reputation { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrendValue ReputationTrend { get; init; }
}