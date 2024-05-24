namespace Observatory.Framework.Files.Journal.Other;

public class LaunchSRV : JournalBase
{
    public override string Event => "LaunchSRV";
    public string Loadout { get; init; }
    public ulong ID { get; init; }
    public bool PlayerControlled { get; init; }
    public string SRVType { get; init; }
    public string SRVType_Localised { get; init; }
}