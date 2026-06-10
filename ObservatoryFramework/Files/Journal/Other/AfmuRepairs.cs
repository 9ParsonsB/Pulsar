namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when repairing modules using the Auto Field Maintenance Unit (AFMU).
/// </summary>
public class AfmuRepairs : JournalBase
{
    public string Module { get; init; }
    public string Module_Localised { get; init; }
    public bool FullyRepaired { get; init; }
    public float Health { get; init; }
}