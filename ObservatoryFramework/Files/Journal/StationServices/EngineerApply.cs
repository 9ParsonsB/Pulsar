namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when applying an engineer blueprint in legacy data.
/// </summary>
[Obsolete("This event was removed in Elite Dangerous 3.0 and will only appear in legacy data.")]
public class EngineerApply : JournalBase
{
    public string Engineer { get; init; }
    public string Blueprint { get; init; }
    public int Level { get; init; }
}