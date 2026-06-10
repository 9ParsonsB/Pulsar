namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when a mission has been abandoned.
/// </summary>
public class MissionAbandoned : JournalBase
{
    public string Name { get; init; }
    public ulong MissionID { get; init; }
    public long Fine { get; init; }
}