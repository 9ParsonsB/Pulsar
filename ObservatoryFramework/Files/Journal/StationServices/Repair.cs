namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when repairing the ship.
/// </summary>
public class Repair : JournalBase
{
    public string Item { get; init; }
    public int Cost { get; init; }
    public IList<string> Items { get; init; }
}