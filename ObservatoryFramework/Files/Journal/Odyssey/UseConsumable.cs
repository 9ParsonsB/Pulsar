namespace Observatory.Framework.Files.Journal.Odyssey;

public class UseConsumable : JournalBase
{
    public override string Event => "UseConsumable";
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public string Type { get; init; }
}