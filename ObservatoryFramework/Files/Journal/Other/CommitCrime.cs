namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when a crime is recorded against the player.
/// </summary>
public class CommitCrime : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CrimeType CrimeType { get; init; }

    public string Faction { get; init; }
    public string Victim { get; init; }
    public string Victim_Localised { get; init; }
    public int Fine { get; init; }
    public int Bounty { get; init; }
}