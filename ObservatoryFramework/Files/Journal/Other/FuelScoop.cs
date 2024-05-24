namespace Observatory.Framework.Files.Journal.Other;

public class FuelScoop : JournalBase
{
    public override string Event => "FuelScoop";
    public float Scooped { get; init; }
    public float Total { get; init; }
}