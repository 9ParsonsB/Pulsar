namespace Observatory.Framework.Files.Journal.Odyssey;

public class CancelTaxi : JournalBase
{
    public override string Event => "CancelTaxi";
    public int Refund { get; init; }
}