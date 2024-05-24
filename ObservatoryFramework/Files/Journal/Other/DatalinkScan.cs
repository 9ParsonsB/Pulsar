namespace Observatory.Framework.Files.Journal.Other;

public class DatalinkScan : JournalBase
{
    public override string Event => "DatalinkScan";
    public string Message { get; init; }
    public string Message_Localised { get; init; }
}