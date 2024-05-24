namespace Observatory.Framework.Files.Journal.Travel;

public class Undocked : JournalBase
{
    public override string Event => "Undocked";
    /// <summary>
    /// Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }
    public string StationType { get; init; }
    public ulong MarketID { get; init; }
    public bool Taxi { get; init; }
    public bool Multicrew { get; init; }
}