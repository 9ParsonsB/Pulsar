namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when a mission is updated with a new destination.
/// </summary>
public class MissionRedirected : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public ulong MissionID { get; init; }
    public string NewDestinationStation { get; init; }
    public string OldDestinationStation { get; init; }
    public string NewDestinationSystem { get; init; }
    public string OldDestinationSystem { get; init; }
}