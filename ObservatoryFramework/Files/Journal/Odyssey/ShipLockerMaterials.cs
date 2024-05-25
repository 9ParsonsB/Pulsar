using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class ShipLockerMaterials : JournalBase
{
    public override string Event => "ShipLockerMaterials";
    public IReadOnlyCollection<BackpackItem> Items { get; init; }
    public IReadOnlyCollection<BackpackItem> Components { get; init; }
    public IReadOnlyCollection<BackpackItem> Consumables { get; init; }
    public IReadOnlyCollection<BackpackItem> Data { get; init; }
}