namespace Observatory.Framework.Files.Journal.Startup;

using System.Text.Json.Serialization;

/// <summary>
///     Written at the start of each journal file as the header event.
/// </summary>
public class FileHeader : JournalBase
{
    [JsonPropertyName("part")]
    public int Part { get; init; }

    [JsonPropertyName("language")]
    public string Language { get; init; }

    [JsonPropertyName("gameversion")]
    public string GameVersion { get; init; }

    [JsonPropertyName("build")]
    public string Build { get; init; }

    public bool Odyssey { get; init; }
}