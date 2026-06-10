namespace Observatory.Framework.Files.Journal.FleetCarrier;

/// <summary>
///     Written when fleet carrier decommissioning is cancelled.
/// </summary>
public class CarrierCancelDecommission : JournalBase
{
    public ulong CarrierID { get; init; }
}