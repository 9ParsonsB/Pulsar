namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierTradeOrder : JournalBase
{
    public override string Event => "CarrierTradeOrder";
    public ulong CarrierID { get; init; }
    public bool BlackMarket { get; init; }
    public string Commodity { get; init; }
    public string Commodity_Localised { get; init; }
    public int PurchaseOrder { get; init; }
    public int SaleOrder { get; init; }
    public bool CancelTrade { get; init; }
    public int Price { get; init; }
}