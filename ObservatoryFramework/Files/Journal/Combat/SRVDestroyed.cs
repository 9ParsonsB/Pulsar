namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when the player's SRV is destroyed.
/// </summary>
public class SRVDestroyed : JournalBase
{
    public string SRVType { get; init; }
    public string SRVType_Localised { get; init; }
}