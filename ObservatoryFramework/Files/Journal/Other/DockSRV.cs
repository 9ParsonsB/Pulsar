namespace Observatory.Framework.Files.Journal.Other;

public class DockSRV : DockFighter
{
    public override string Event => "DockSRV";
    public string SRVType { get; init; }
    public string SRVType_Localised { get; init; }
}