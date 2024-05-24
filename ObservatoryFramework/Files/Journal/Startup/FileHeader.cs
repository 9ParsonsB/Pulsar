using System.Text.Json.Serialization;

namespace Observatory.Framework.Files.Journal.Startup;

public class FileHeader : JournalBase
{
    public override string Event => "FileHeader";
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