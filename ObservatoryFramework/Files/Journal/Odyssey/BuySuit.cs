namespace Observatory.Framework.Files.Journal.Odyssey;

public class BuySuit : JournalBase
{
    public override string Event => "BuySuit";
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public int Price { get; init; }
    public ulong SuitID { get; init; }
}