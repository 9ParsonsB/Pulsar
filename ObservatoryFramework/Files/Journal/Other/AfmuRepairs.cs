namespace Observatory.Framework.Files.Journal.Other;

public class AfmuRepairs : JournalBase
{
    public override string Event => "AfmuRepairs";
    public string Module { get; init; }
    public string Module_Localised { get; init; }
    public bool FullyRepaired { get; init; }
    public float Health { get; init; }
}