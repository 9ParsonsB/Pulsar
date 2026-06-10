namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when switching control between the main ship and a fighter.
/// </summary>
public class VehicleSwitch : JournalBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VehicleSwitchTo To { get; init; }
}