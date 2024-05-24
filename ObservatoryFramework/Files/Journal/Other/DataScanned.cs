namespace Observatory.Framework.Files.Journal.Other;

public class DataScanned : JournalBase
{
    public override string Event => "DataScanned";
    public string Type { get; init; }
    public string Type_Localised { get; init; }
}