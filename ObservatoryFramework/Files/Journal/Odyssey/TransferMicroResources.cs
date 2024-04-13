using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class TransferMicroResources : JournalBase
{
    public ImmutableList<MicroTransfer> Transfers { get; init; }
}