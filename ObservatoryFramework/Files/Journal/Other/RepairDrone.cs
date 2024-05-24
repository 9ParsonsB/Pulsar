namespace Observatory.Framework.Files.Journal.Other;

public class RepairDrone : JournalBase
{
    public override string Event => "RepairDrone";
    public float HullRepaired { get; init; }
    public float CockpitRepaired { get; init; }
    public float CorrosionRepaired { get; init; }
}