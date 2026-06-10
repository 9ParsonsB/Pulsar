namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when changing the task assignment of a member of crew.
/// </summary>
public class CrewAssign : CrewFire
{
    public string Role { get; init; }
}