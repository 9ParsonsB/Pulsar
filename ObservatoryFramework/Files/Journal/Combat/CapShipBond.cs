namespace Observatory.Framework.Files.Journal.Combat;

public class CapShipBond : JournalBase
{
    public override string Event => "CapShipBond";
    public long Reward { get; init; }
    public string AwardingFaction { get; init; }
    public string VictimFaction { get; init; }
}