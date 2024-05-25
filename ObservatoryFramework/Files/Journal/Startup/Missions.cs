using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Startup;

public class Missions : JournalBase
{
    public override string Event => "Missions";
    public List<Mission> Active { get; init; }
    public List<Mission> Failed { get; init; }
    public List<Mission> Complete { get; init; }
}