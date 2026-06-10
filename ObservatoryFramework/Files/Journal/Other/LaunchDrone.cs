namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when using any type of drone/limpet.
/// </summary>
public class LaunchDrone : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LimpetDrone Type { get; init; }
}