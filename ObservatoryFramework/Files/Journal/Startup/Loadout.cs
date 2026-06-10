namespace Observatory.Framework.Files.Journal.Startup;

using ParameterTypes;

/// <summary>
///     Written at startup, when loading from main menu, or when switching ships, or after changing the ship in Outfitting,
///     or when docking SRV back in mothership.
/// </summary>
public class Loadout : JournalBase
{
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
    public string ShipName { get; init; }
    public string ShipIdent { get; init; }
    public int? CargoCapacity { get; init; }
    public ulong? HullValue { get; init; }
    public ulong? ModulesValue { get; init; }
    public double? HullHealth { get; init; }
    public double? UnladenMass { get; init; }
    public FuelCapacity? FuelCapacity { get; init; }
    public double? MaxJumpRange { get; init; }
    public ulong? Rebuy { get; init; }
    public bool? Hot { get; init; }
    public List<Modules> Modules { get; init; }
}