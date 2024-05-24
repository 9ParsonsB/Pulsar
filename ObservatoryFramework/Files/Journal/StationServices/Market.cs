using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class Market : JournalBase
{
    public override string Event => "Market";
    public ulong MarketID { get; init; }
    /// <summary>
    /// Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }
    public string StationType { get; init; }
    public string StarSystem { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarrierDockingAccess CarrierDockingAccess { get; init; }
}