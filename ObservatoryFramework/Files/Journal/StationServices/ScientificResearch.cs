namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when contributing materials to a "research" community goal.
/// </summary>
public class ScientificResearch : JournalBase
{
    public ulong MarketID { get; init; }
    public string Name { get; init; }
    public string Category { get; init; }
    public int Count { get; init; }
}