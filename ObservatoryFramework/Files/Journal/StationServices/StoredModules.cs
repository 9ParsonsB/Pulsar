using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class StoredModules : JournalBase
{
    public override string Event => "StoredModules";
    public string StarSystem { get; init; }
    /// <summary>
    /// Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }
    public ulong MarketID { get; init; }
    public List<StoredItem> Items { get; init; }
}