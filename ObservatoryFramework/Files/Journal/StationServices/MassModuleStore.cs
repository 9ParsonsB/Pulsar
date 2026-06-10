namespace Observatory.Framework.Files.Journal.StationServices;

using ParameterTypes;

/// <summary>
///     Written when putting multiple modules into storage.
/// </summary>
public class MassModuleStore : JournalBase
{
    public ulong MarketID { get; init; }
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
    public List<Item> Items { get; init; }
}