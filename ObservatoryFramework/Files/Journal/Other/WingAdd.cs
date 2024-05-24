namespace Observatory.Framework.Files.Journal.Other;

public class WingAdd : JournalBase
{
    public override string Event => "WingAdd";
    public string Name { get; init; }
}