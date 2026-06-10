namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when buying microresources.
/// </summary>
public class BuyMicroResources : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MicroCategory Category { get; init; }

    public int Count { get; init; }
    public int Price { get; init; }
    public ulong MarketID { get; init; }
    public int TotalCount { get; init; }
    public List<MicroResource> MicroResources { get; init; }
}