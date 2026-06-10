namespace Observatory.Framework.Files.Journal.FleetCarrier;

using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when fleet carrier docking permissions are changed.
/// </summary>
public class CarrierDockingPermission : JournalBase
{
    public ulong CarrierID { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarrierDockingAccess DockingAccess { get; init; }

    public bool AllowNotorious { get; init; }
}