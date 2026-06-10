namespace Observatory.Framework.Files.ParameterTypes;

using System.Text.Json.Serialization;

public class EffectType
{
    public string Effect { get; init; }
    public string Effect_Localised { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrendValue Trend { get; set; }
}