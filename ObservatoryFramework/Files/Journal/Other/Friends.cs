namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when receiving information about a change in a friend's status.
/// </summary>
public class Friends : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FriendStatus Status { get; init; }

    public string Name { get; init; }
}