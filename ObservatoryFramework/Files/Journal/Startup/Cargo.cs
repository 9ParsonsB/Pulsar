using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Startup;

public class Cargo : JournalBase
{
    public override string Event => "Cargo";
    public string Vessel { get; init; }
    public int Count { get; init; }
    public IReadOnlyCollection<CargoType> Inventory { get; init; }
}