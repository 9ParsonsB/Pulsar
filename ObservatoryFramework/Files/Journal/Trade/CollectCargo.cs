namespace Observatory.Framework.Files.Journal.Trade;

/// <summary>
///     Written when scooping cargo from space or planet surface.
/// </summary>
public class CollectCargo : JournalBase
{
    public string Type { get; init; }
    public string Type_Localised { get; init; }
    public bool Stolen { get; init; }
    public int MissionID { get; init; }
}