namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when purchasing an SRV or Fighter.
/// </summary>
public class RestockVehicle : JournalBase
{
    public string Type { get; init; }
    public string Loadout { get; init; }
    public int Cost { get; init; }
    public int Count { get; init; }
}