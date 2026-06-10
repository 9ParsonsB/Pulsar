namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when a ship's fighter is rebuilt in the hangar.
/// </summary>
public class FighterRebuilt : JournalBase
{
    public string Loadout { get; init; }
    public ulong ID { get; init; }
}