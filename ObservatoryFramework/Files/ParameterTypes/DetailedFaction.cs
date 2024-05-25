using System.Collections.Immutable;

namespace Observatory.Framework.Files.ParameterTypes;

public class DetailedFaction : Faction
{
    public string Government { get; init; }
    public float Influence { get; init; }
    public string Allegiance { get; init; }
    public string Happiness { get; init; }
    public string Happiness_Localised { get; init; }
    public float MyReputation { get; init; }
    public IReadOnlyCollection<FactionStateTrend> RecoveringStates { get; init; }
    public IReadOnlyCollection<FactionState> ActiveStates { get; init; }
}