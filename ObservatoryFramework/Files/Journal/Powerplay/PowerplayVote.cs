namespace Observatory.Framework.Files.Journal.Powerplay;

using System.Text.Json.Serialization;

/// <summary>
///     Written when voting for a system expansion.
/// </summary>
public class PowerplayVote : PowerplayJoin
{
    public int Votes { get; init; }

    [JsonPropertyName("")]
    public int UnnamedValue { get; init; }

    public string System { get; init; }
}