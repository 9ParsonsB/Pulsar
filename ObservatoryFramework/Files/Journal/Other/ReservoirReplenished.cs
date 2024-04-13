namespace Observatory.Framework.Files.Journal.Other;

public class ReservoirReplenished : JournalBase
{
    public float FuelMain { get; init; }
    public float FuelReservoir { get; init; }
}