namespace Observatory.Framework.Files.Journal.Trade;

public class MiningRefined : JournalBase
{
    public override string Event => "MiningRefined";
    public string Type { get; init; }
    public string Type_Localised { get; init; }
}