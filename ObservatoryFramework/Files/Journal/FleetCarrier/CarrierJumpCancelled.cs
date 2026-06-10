namespace Observatory.Framework.Files.Journal.FleetCarrier;

/// <summary>
///     Written when a jump is cancelled.
/// </summary>
public class CarrierJumpCancelled : JournalBase
{
    public ulong CarrierID { get; init; }
}