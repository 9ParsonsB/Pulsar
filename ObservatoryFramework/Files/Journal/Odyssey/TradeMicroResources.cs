using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class TradeMicroResources : JournalBase
{
    public override string Event => "TradeMicroResources";
    public List<MicroResource> Offered { get; init; }
    public string Received { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MicroCategory Category { get; init; }
    public int Count { get; init; }
    public ulong MarketID { get; init; }
}