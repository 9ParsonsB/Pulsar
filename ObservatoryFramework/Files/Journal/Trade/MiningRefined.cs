namespace Observatory.Framework.Files.Journal.Trade;

/// <summary>
///     Written when mining fragments are converted unto a unit of cargo by refinery.
/// </summary>
public class MiningRefined : JournalBase
{
    public string Type { get; init; }
    public string Type_Localised { get; init; }
}