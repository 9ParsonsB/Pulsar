using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class BackpackChange : JournalBase
{
    public override string Event => "BackpackChange";
    public IReadOnlyCollection<BackpackItemChange> Added { get; init; }
    public IReadOnlyCollection<BackpackItemChange> Removed { get; init; }
}