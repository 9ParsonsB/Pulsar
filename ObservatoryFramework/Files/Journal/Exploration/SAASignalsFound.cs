using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Exploration;

/// <summary>
/// Event written when the surface scan finds signals on the body.
/// </summary>
public class SAASignalsFound : JournalBase
{
    public override string Event => "SAASignalsFound";
    /// <summary>
    /// Unique ID of current system.
    /// </summary>
    public ulong SystemAddress { get; init; }
    /// <summary>
    /// Name of the scanned body.
    /// </summary>
    public string BodyName { get; init; }
    /// <summary>
    /// ID of the body within the system.
    /// </summary>
    public int BodyID { get; init; }
    /// <summary>
    /// List of signals found.
    /// </summary>
    public IReadOnlyCollection<Signal> Signals { get; init; }
    /// <summary>
    /// List of genuses present.
    /// </summary>
    public IReadOnlyCollection<GenusType> Genuses { get; init; }
}