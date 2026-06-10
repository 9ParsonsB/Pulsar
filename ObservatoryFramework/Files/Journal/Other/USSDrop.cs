namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when dropping from Supercruise at a USS.
/// </summary>
public class USSDrop : JournalBase
{
    public string USSType { get; init; }
    public string USSType_Localised { get; init; }
    public int USSThreat { get; init; }
}