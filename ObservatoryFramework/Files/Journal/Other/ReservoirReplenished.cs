namespace Observatory.Framework.Files.Journal.Other;

public class ReservoirReplenished : JournalBase
{
    public override string Event => "ReservoirReplenished";
    public float FuelMain { get; init; }
    public float FuelReservoir { get; init; }
}