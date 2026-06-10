namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when scanning a datalink generates a reward.
/// </summary>
public class DatalinkVoucher : JournalBase
{
    public int Reward { get; init; }
    public string VictimFaction { get; init; }
    public string PayeeFaction { get; init; }
}