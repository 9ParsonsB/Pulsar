using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class CommunityGoal : JournalBase
{
    public override string Event => "CommunityGoal";
    public IReadOnlyCollection<CurrentGoal> CurrentGoals { get; init; }
}