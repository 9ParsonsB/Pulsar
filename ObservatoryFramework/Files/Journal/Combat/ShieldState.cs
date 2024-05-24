namespace Observatory.Framework.Files.Journal.Combat;

public class ShieldState : JournalBase
{
    public override string Event => "ShieldState";
    public bool ShieldsUp { get; init; }
}