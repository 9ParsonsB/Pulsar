namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when a gas giant ring hotspot reservoir is replenished.
/// </summary>
public class ReservoirReplenished : JournalBase
{
    public float FuelMain { get; init; }
    public float FuelReservoir { get; init; }
}