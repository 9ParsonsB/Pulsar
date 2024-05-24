namespace Observatory.Framework.Files.Journal.StationServices;

public class RepairAll : JournalBase
{
    public override string Event => "RepairAll";
    public int Cost { get; init; }
}