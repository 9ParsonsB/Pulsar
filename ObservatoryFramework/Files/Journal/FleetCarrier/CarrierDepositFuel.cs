namespace Observatory.Framework.Files.Journal.FleetCarrier;

/// <summary>
///     Written when fuel is deposited into a fleet carrier.
/// </summary>
public class CarrierDepositFuel : JournalBase
{
    public ulong CarrierID { get; init; }
    public int Amount { get; init; }
    public int Total { get; init; }
}