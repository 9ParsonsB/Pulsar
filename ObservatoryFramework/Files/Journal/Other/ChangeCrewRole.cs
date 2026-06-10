namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when in a crew on someone else's ship, player switched crew role.
/// </summary>
public class ChangeCrewRole : JournalBase
{
    public string Role { get; init; }
    public bool Telepresence { get; init; }
}