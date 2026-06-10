namespace Observatory.Framework.Files.Journal.Travel;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when a station denies a docking request.
/// </summary>
public class DockingDenied : DockingCancelled
{
    /// <summary>
    ///     Reason the docking request was denied.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Reason Reason { get; init; }
}