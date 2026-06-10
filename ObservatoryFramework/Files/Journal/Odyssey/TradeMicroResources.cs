namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when the player exchanges owned microresources to receive some other type of microresource.
/// </summary>
public class TradeMicroResources : JournalBase
{
    public List<MicroResource> Offered { get; init; }
    public string Received { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MicroCategory Category { get; init; }

    public int Count { get; init; }
    public ulong MarketID { get; init; }
}