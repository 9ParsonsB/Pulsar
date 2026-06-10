namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;

/// <summary>
///     Written when an NPC crew member rank changes.
/// </summary>
public class NpcCrewRank : JournalBase
{
    public ulong NpcCrewId { get; init; }
    public string NpcCrewName { get; init; }
    public RankCombat RankCombat { get; init; }
}