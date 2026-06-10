namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the current hyperspace jump target is set or updated.
/// </summary>
public class FSDTarget : JournalBase
{
    /// <summary>
    ///     Name of the target star system.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Unique address of the target star system.
    /// </summary>
    public ulong SystemAddress { get; init; }

    /// <summary>
    ///     Spectral class of the target star.
    /// </summary>
    public string StarClass { get; init; }

    /// <summary>
    ///     Number of jumps remaining in the plotted route after this target.
    /// </summary>
    public int RemainingJumpsInRoute { get; init; }
}