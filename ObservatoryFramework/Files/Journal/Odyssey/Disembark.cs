namespace Observatory.Framework.Files.Journal.Odyssey;

public class Disembark : JournalBase
{
    public override string Event => "Disembark";
    public bool SRV { get; init; }
    public bool Taxi { get; init; }
    public bool Multicrew { get; init; }
    public ulong ID { get; init; }
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }
    public string Body { get; init; }
    public int BodyID { get; init; }
    public bool OnStation { get; init; }
    public bool OnPlanet { get; init; }
    /// <summary>
    /// Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }
    public string StationType { get; init; }
    public ulong MarketID { get; init; }
}