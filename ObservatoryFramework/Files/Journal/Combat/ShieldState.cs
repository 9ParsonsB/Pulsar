namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when shields are disabled in combat, or recharged.
/// </summary>
public class ShieldState : JournalBase
{
    public bool ShieldsUp { get; init; }
}