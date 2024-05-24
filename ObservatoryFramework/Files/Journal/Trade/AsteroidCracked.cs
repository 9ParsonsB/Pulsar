namespace Observatory.Framework.Files.Journal.Trade;

public class AsteroidCracked : JournalBase
{
    public override string Event => "AsteroidCracked";
    public string Body { get; init; }
}