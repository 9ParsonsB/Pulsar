namespace Observatory.Framework.Files.Journal.StationServices;

[Obsolete(JournalUtilities.ObsoleteMessage)]
public class PayLegacyFines : JournalBase
{
    public override string Event => "PayLegacyFines";
    public long Amount { get; init; }
    public float BrokerPercentage { get; init; }
}