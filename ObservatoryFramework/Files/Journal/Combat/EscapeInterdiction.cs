namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when Player has escaped interdiction.
/// </summary>
public class EscapeInterdiction : JournalBase
{
    public string Interdictor { get; init; }
    public bool IsPlayer { get; init; }
    public bool IsThargoid { get; init; }
}