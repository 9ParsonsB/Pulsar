using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class CreateSuitLoadout : DeleteSuitLoadout
{
    public override string Event => "CreateSuitLoadout";
    public List<SuitModule> Modules { get; init; }
    public IList<string> SuitMods { get; init; }
}