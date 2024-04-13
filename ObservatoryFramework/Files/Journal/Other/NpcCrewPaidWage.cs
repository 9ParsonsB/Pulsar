namespace Observatory.Framework.Files.Journal.Other;

public class NpcCrewPaidWage : JournalBase
{
    public ulong NpcCrewId { get; init; }
    public string NpcCrewName { get; init; }
    public int Amount { get; init; }
}