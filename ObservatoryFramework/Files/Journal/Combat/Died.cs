using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Combat;

public class Died : JournalBase
{
    public override string Event => "Died";
    public string KillerName { get; init; }
    public string KillerName_Localised { get; init; }
    public string KillerShip { get; init; }
    public string KillerRank { get; init; }
    public IReadOnlyCollection<Killer> Killers { get; init; }
}