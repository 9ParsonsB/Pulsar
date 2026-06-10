namespace Observatory.Framework.Files.ParameterTypes;

using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

[Owned]
public class Mining
{
    [JsonPropertyName("Mining_Profits")]
    public long MiningProfits { get; init; }

    [JsonPropertyName("Quantity_Mined")]
    public long QuantityMined { get; init; }

    [JsonPropertyName("Materials_Collected")]
    public long MaterialsCollected { get; init; }
}