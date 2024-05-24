using Observatory.Framework.Files.Journal.Travel;

namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class FCMaterials : FSDJump
{
    public override string Event => "FCMaterials";
    public ulong MarketID { get; init; }
    public string CarrierName { get; init; }
    public ulong CarrierID { get; init; }
}