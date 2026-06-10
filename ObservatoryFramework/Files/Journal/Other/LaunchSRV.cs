namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when deploying the SRV from a ship onto planet surface.
/// </summary>
public class LaunchSRV : JournalBase
{
    public string Loadout { get; init; }
    public ulong ID { get; init; }
    public bool PlayerControlled { get; init; }
    public string SRVType { get; init; }
    public string SRVType_Localised { get; init; }
}