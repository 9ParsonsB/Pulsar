namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when a text message is received from another player or npc.
/// </summary>
public class ReceiveText : JournalBase
{
    public string From { get; init; }
    public string? From_Localised { get; init; }
    public string Message { get; init; }
    public string? Message_Localised { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TextChannel Channel { get; init; }
}