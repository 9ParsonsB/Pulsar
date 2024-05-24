using System.Text.Json.Serialization;

namespace Observatory.Framework.Files.Journal.Powerplay;

public class PowerplayVote : PowerplayJoin
{
    public override string Event => "PowerplayVote";
    public int Votes { get; init; }
    [JsonPropertyName("")]
    public int UnnamedValue { get; init; }
    public string System { get; init; }
}