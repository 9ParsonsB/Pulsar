namespace Observatory.Framework.Files.Journal.Other;

public class NpcCrewPaidWage : JournalBase
{
    public override string Event => "NpcCrewPaidWage";
    public ulong NpcCrewId { get; init; }
    public string NpcCrewName { get; init; }
    public int Amount { get; init; }
}