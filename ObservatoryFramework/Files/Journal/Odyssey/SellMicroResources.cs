using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class SellMicroResources : JournalBase
{
    public override string Event => "SellMicroResources";
    public List<MicroResource> MicroResources { get; init; }
    public int Price { get; init; }
    public ulong MarketID { get; init; }
}