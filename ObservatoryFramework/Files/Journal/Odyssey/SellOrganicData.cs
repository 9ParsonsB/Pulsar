using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class SellOrganicData : JournalBase
{
    public ulong MarketID { get; init; }
    public ImmutableList<BioData> BioData { get; init; }
}