using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class ScanOrganic : JournalBase
{
    public override string Event => "ScanOrganic";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScanOrganicType ScanType { get; init; }
    public string Genus { get; init; }
    public string Genus_Localised { get; init; }
    public string Species { get; init; }
    public string Species_Localised { get; init; }
    public string Variant {  get; init; }
    public string Variant_Localised { get; init; }
    public ulong SystemAddress { get; init; }
    public int Body { get; init; }
}