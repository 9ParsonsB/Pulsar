namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;

/// <summary>
///     Written when microresources are transferred between the backpack and ship locker.
/// </summary>
public class TransferMicroResources : JournalBase
{
    public List<MicroTransfer> Transfers { get; init; }
}