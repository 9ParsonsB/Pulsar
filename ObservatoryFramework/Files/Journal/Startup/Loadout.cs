using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Startup;

public class Loadout : JournalBase
{
    public override string Event => "Loadout";
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
    public string ShipName { get; init; }
    public string ShipIdent { get; init; }
    public int CargoCapacity { get; init; }
    public ulong HullValue { get; init; }
    public ulong ModulesValue { get; init; }
    public double HullHealth { get; init; }
    public double UnladenMass { get; init; }
    public FuelCapacity FuelCapacity { get; init; }
    public double MaxJumpRange { get; init; }
    public ulong Rebuy { get; init; }
    public bool Hot { get; init; }
    public List<Modules> Modules { get; init; }
}