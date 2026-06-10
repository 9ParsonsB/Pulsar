namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when dismissing a member of crew.
/// </summary>
public class CrewFire : JournalBase
{
    public string Name { get; init; }
    public ulong CrewID { get; init; }
}