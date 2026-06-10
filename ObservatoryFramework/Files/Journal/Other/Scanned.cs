namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when the player's ship has been scanned.
/// </summary>
public class Scanned : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScanType ScanType { get; init; }
}