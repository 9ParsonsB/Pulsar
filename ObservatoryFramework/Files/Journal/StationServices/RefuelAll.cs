namespace Observatory.Framework.Files.Journal.StationServices;

public class RefuelAll : JournalBase
{
    public override string Event => "RefuelAll";
    public int Cost { get; init; }
    public float Amount { get; init; }
}