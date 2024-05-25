using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class SellOrganicData : JournalBase
{
    public override string Event => "SellOrganicData";
    public ulong MarketID { get; init; }
    public List<BioData> BioData { get; init; }
}