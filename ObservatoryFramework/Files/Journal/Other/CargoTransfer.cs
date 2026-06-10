namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;

/// <summary>
///     Written when cargo is transferred between the ship, SRV, or fleet carrier.
/// </summary>
public class CargoTransfer : JournalBase
{
    public List<CargoTransferDetail> Transfers { get; init; }
}