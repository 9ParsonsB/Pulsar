namespace Observatory.Framework.Files.Journal.Powerplay;

public class PowerplayJoin : JournalBase
{
    public override string Event => "PowerplayJoin";
    public string Power { get; init; }
}