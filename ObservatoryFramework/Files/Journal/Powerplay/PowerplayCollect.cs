namespace Observatory.Framework.Files.Journal.Powerplay;

public class PowerplayCollect : PowerplayJoin
{
    public override string Event => "PowerplayCollect";
    public string Type { get; init; }
    public string Type_Localised { get; init; }
    public int Count { get; init; }
}