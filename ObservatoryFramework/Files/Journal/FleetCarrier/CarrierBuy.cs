namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierBuy : JournalBase
{
    public override string Event => "CarrierBuy";
    public long BoughtAtMarket { get; init; }
    public ulong SystemAddress { get; init; }
    public ulong CarrierID { get; init; }
    public string Location { get; init; }
    public long Price { get; init; }
    public string Variant { get; init; }
    public string Callsign { get; init; }
}