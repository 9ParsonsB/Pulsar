namespace Observatory.Framework.Files.Journal.Odyssey;

public class CollectItems : JournalBase
{
    public override string Event => "CollectItems";
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public string Type { get; init; }
    public ulong OwnerID { get; init; }
    public int Count { get; init; }
    public bool Stolen { get; init; }
}