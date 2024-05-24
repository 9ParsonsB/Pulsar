namespace Observatory.Framework.Files.Journal.Combat;

public class EscapeInterdiction : JournalBase
{
    public override string Event => "EscapeInterdiction";
    public string Interdictor { get; init; }
    public bool IsPlayer { get; init; }
    public bool IsThargoid { get; init; }
}