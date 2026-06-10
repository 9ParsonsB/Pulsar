namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when a ship-launched fighter is destroyed.
/// </summary>
public class FighterDestroyed : JournalBase
{
    public ulong ID { get; init; }
}