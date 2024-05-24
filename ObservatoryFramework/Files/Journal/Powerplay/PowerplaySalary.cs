namespace Observatory.Framework.Files.Journal.Powerplay;

public class PowerplaySalary : PowerplayJoin
{
    public override string Event => "PowerplaySalary";
    public int Amount { get; init; }
}