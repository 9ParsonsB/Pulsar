namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when the player's ship has been repaired by a repair drone.
/// </summary>
public class RepairDrone : JournalBase
{
    public float HullRepaired { get; init; }
    public float CockpitRepaired { get; init; }
    public float CorrosionRepaired { get; init; }
}