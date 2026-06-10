namespace Observatory.Framework.Files.ParameterTypes;

public class Faction
{
    public string Name { get; init; }
    public string? FactionState { get; init; }
    public string? Government { get; init; }
    public float? Influence { get; init; }
    public string? Allegiance { get; init; }
    public string? Happiness { get; init; }
    public string? Happiness_Localised { get; init; }
    public float? MyReputation { get; init; }
    public List<FactionStateTrend>? RecoveringStates { get; init; }
    public List<FactionState>? ActiveStates { get; init; }
    public List<FactionStateTrend>? PendingStates { get; init; }
    public bool? SquadronFaction { get; init; }
    public bool? HappiestSystem { get; init; }
    public bool? HomeSystem { get; init; }
}