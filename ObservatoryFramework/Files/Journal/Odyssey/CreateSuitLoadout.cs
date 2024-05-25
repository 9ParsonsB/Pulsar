using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class CreateSuitLoadout : DeleteSuitLoadout
{
    public override string Event => "CreateSuitLoadout";
    public IReadOnlyCollection<SuitModule> Modules { get; init; }
    public ICollection<string> SuitMods { get; init; }
}