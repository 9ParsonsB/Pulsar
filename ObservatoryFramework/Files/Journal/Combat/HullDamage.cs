namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when hull health drops below a threshold (20% steps).
/// </summary>
public class HullDamage : JournalBase
{
    public float Health { get; init; }
    public bool PlayerPilot { get; init; }
    public bool Fighter { get; init; }
}