using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class BackpackChange : JournalBase
{
    public ImmutableList<BackpackItemChange> Added { get; init; }
    public ImmutableList<BackpackItemChange> Removed { get; init; }
}