namespace Observatory.Framework.Files.Journal.Combat;

public class FactionKillBond : JournalBase
{
    public override string Event => "FactionKillBond";
    public long Reward { get; init; }
    public string AwardingFaction { get; init; }
    public string AwardingFaction_Localised { get; init; }
    public string VictimFaction { get; init; }
    public string VictimFaction_Localised { get; init; }
}