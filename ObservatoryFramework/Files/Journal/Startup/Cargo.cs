namespace Observatory.Framework.Files.Journal.Startup;

using ParameterTypes;

/// <summary>
///     Written at startup, _note this is now written slightly later in startup, after we have initialised missions, so we
///     can detect if any cargo came from an abandoned delivery mission_.
/// </summary>
public class Cargo : JournalBase
{
    public string Vessel { get; init; }
    public int Count { get; init; }
    public List<CargoType> Inventory { get; init; }
}