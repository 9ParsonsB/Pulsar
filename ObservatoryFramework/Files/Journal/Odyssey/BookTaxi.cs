namespace Observatory.Framework.Files.Journal.Odyssey;

public class BookTaxi : JournalBase
{
    public override string Event => "BookTaxi";
    public int Cost { get; init; }
    public string DestinationSystem { get; init; }
    public string DestinationLocation { get; init; }
    public bool Retreat { get; init; }
}