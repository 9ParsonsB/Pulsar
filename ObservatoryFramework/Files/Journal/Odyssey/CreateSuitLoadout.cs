using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class CreateSuitLoadout : DeleteSuitLoadout
{
    public override string Event => "CreateSuitLoadout";
    public ImmutableList<SuitModule> Modules { get; init; }
    public ImmutableList<string> SuitMods { get; init; }
}