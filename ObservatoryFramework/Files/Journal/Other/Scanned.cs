using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class Scanned : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScanType ScanType { get; init; }
}