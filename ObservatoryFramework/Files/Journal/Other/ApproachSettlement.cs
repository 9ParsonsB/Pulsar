namespace Observatory.Framework.Files.Journal.Other;

using Converters;
using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when approaching a planetary settlement.
/// </summary>
public class ApproachSettlement : JournalBase
{
    public ulong SystemAddress { get; init; }
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public ulong MarketID { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
    public int BodyID { get; init; }
    public string BodyName { get; init; }
    public List<StationEconomy> StationEconomies { get; init; }
    public string StationEconomy { get; init; }
    public string StationEconomy_Localised { get; init; }
    public Faction StationFaction { get; init; }
    public string StationGovernment { get; init; }
    public string StationGovernment_Localised { get; init; }

    [JsonConverter(typeof(StationServiceConverter))]
    public StationService StationServices { get; init; }
}