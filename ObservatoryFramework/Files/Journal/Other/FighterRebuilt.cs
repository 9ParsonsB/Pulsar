namespace Observatory.Framework.Files.Journal.Other;

public class FighterRebuilt : JournalBase
{
    public override string Event => "FighterRebuilt";
    public string Loadout { get; init; }
    public ulong ID { get; init; }
}