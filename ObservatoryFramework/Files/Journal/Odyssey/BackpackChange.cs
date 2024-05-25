using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class BackpackChange : JournalBase
{
    public override string Event => "BackpackChange";
    public List<BackpackItemChange> Added { get; init; }
    public List<BackpackItemChange> Removed { get; init; }
}