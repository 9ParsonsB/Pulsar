namespace Observatory.Framework.Files.Journal.Combat;

public class EscapeInterdiction : JournalBase
{
    public string Interdictor { get; init; }
    public bool IsPlayer { get; init; }
    public bool IsThargoid { get; init; }
}