namespace Observatory.Framework.Files.Journal.Combat;

public class FighterDestroyed : JournalBase
{
    public override string Event => "FighterDestroyed";
    public ulong ID { get; init; }
}