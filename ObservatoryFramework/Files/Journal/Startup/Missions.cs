using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Startup;

public class Missions : JournalBase
{
    public override string Event => "Missions";
    public IReadOnlyCollection<Mission> Active { get; init; }
    public IReadOnlyCollection<Mission> Failed { get; init; }
    public IReadOnlyCollection<Mission> Complete { get; init; }
}