using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class BackpackMaterials : JournalBase
{
    public override string Event => "BackpackMaterials";
    public List<BackpackItem> Items { get; init; }
    public List<BackpackItem> Components { get; init; }
    public List<BackpackItem> Consumables { get; init; }
    public List<BackpackItem> Data { get; init; }
}