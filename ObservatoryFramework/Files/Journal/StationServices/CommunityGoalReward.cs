namespace Observatory.Framework.Files.Journal.StationServices;

public class CommunityGoalReward : CommunityGoalDiscard
{
    public override string Event => "CommunityGoalReward";
    public long Reward { get; init; }
}