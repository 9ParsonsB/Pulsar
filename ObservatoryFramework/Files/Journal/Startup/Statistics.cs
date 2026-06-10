namespace Observatory.Framework.Files.Journal.Startup;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written at startup.
/// </summary>
public class Statistics : JournalBase
{
    [JsonPropertyName("Bank_Account")]
    public BankAccount BankAccount { get; init; }

    public Combat Combat { get; init; }
    public Crime Crime { get; init; }
    public Smuggling Smuggling { get; init; }
    public Trading Trading { get; init; }
    public Mining Mining { get; init; }
    public Exploration Exploration { get; init; }
    public ParameterTypes.Passengers Passengers { get; init; }

    [JsonPropertyName("Search_And_Rescue")]
    public SearchAndRescue SearchAndRescue { get; init; }

    public Crafting Crafting { get; init; }
    public Crew Crew { get; init; }
    public Multicrew Multicrew { get; init; }

    [JsonPropertyName("TG_ENCOUNTERS")]
    public Thargoid? Thargoid { get; init; }

    [JsonPropertyName("Material_Trader_Stats")]
    public MaterialTrader MaterialTrader { get; init; }

    public CQC? CQC { get; init; }

    [JsonPropertyName("FLEETCARRIER")]
    public FleetCarrier? FleetCarrier { get; init; }

    public Exobiology? Exobiology { get; init; }
}