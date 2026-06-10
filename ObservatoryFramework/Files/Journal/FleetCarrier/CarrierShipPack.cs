namespace Observatory.Framework.Files.Journal.FleetCarrier;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when fleet carrier ship packs are bought, sold, or restocked.
/// </summary>
public class CarrierShipPack : JournalBase
{
    public ulong CarrierID { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarrierOperation Operation { get; init; }

    public string PackTheme { get; init; }
    public int PackTier { get; init; }
    public int Cost { get; init; }
    public int Refund { get; init; }
}