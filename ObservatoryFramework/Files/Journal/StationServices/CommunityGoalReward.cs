namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when receiving a reward for a community goal.
/// </summary>
public class CommunityGoalReward : CommunityGoalDiscard
{
    public long Reward { get; init; }
}