using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class RedeemVoucher : JournalBase
{
    public override string Event => "RedeemVoucher";
    [JsonConverter(typeof(VoucherTypeConverter))]
    public VoucherType Type { get; init; }
    public long Amount { get; init; }
    public string Faction { get; init; }
    public float BrokerPercentage { get; init; }
    public ImmutableList<VoucherFaction> Factions { get; init; }

}