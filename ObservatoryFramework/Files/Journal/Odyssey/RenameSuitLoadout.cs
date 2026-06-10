namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when a suit loadout is renamed.
/// </summary>
public class RenameSuitLoadout : JournalBase
{
    public ulong SuitID { get; init; }
    public string SuitName { get; init; }
    public ulong LoadoutID { get; init; }
    public string LoadoutName { get; init; }
}