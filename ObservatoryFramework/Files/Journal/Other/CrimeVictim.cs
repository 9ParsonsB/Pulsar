namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when another player commits a crime against the current player.
/// </summary>
public class CrimeVictim : JournalBase
{
    public string Offender { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CrimeType CrimeType { get; init; }

    public int Fine { get; init; }
    public int Bounty { get; init; }
}