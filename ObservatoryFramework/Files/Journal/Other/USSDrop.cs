namespace Observatory.Framework.Files.Journal.Other;

public class USSDrop : JournalBase
{
    public override string Event => "USSDrop";
    public string USSType { get; init; }
    public string USSType_Localised { get; init; }
    public int USSThreat { get; init; }
}