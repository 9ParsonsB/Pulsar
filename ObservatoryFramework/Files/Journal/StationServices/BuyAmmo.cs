namespace Observatory.Framework.Files.Journal.StationServices;

public class BuyAmmo : JournalBase
{
    public override string Event => "BuyAmmo";
    public int Cost { get; init; }
}