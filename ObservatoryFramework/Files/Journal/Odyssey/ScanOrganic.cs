namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when the player uses the Organic Sampling Tool to scan, log or analyse organic discoveries. The first scan
///     is 'Log', subsequent scans are 'sample' until fully scanned, final scan is 'analyse'.
/// </summary>
public class ScanOrganic : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScanOrganicType ScanType { get; init; }

    public string Genus { get; init; }
    public string Genus_Localised { get; init; }
    public string Species { get; init; }
    public string Species_Localised { get; init; }
    public string Variant { get; init; }
    public string Variant_Localised { get; init; }
    public ulong SystemAddress { get; init; }
    public int Body { get; init; }
}