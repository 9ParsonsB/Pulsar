namespace Observatory.Framework.Files.Journal.Combat;

public class UnderAttack : JournalBase
{
    public override string Event => "UnderAttack";
    public string Target { get; init; }
}