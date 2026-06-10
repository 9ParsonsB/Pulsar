namespace Observatory.Framework.Files.Journal.StationServices;

using Converters;
using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when claiming payment for combat bounties and bonds.
/// </summary>
public class RedeemVoucher : JournalBase
{
    [JsonConverter(typeof(VoucherTypeConverter))]
    public VoucherType Type { get; init; }

    public long Amount { get; init; }
    public string Faction { get; init; }
    public float BrokerPercentage { get; init; }
    public List<VoucherFaction> Factions { get; init; }
}