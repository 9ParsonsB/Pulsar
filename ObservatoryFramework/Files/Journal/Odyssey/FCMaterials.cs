namespace Observatory.Framework.Files.Journal.Odyssey;

public class FCMaterials : JournalBase
{
    public override string Event => "FCMaterials";
    public ulong MarketID { get; init; }
    public string CarrierName { get; init; }
    public string CarrierID { get; init; }
}