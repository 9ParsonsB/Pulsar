namespace Observatory.Framework.Files.Journal.Combat;

public class SRVDestroyed : JournalBase
{
    public override string Event => "SRVDestroyed";
    public string SRVType { get; init; }
    public string SRVType_Localised { get; init; }
}