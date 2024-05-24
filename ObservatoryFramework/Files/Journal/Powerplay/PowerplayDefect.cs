namespace Observatory.Framework.Files.Journal.Powerplay;

public class PowerplayDefect : JournalBase
{
    public override string Event => "PowerplayDefect";
    public string FromPower { get; init; }
    public string ToPower { get; init; }
}