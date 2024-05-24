using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class Scanned : JournalBase
{
    public override string Event => "Scanned";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScanType ScanType { get; init; }
}