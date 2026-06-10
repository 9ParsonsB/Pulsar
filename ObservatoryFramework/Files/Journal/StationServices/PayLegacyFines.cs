namespace Observatory.Framework.Files.Journal.StationServices;

[Obsolete(JournalUtilities.ObsoleteMessage)]
/// <summary>
///     Written when paying legacy fines.
/// </summary>
public class PayLegacyFines : JournalBase
{
    public long Amount { get; init; }
    public float BrokerPercentage { get; init; }
}