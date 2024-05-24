namespace Observatory.Framework.Files.Journal.Other;

public class DatalinkVoucher : JournalBase
{
    public override string Event => "DatalinkVoucher";
    public int Reward { get; init; }
    public string VictimFaction { get; init; }
    public string PayeeFaction { get; init; }
}