namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when docking an SRV with the ship.
/// </summary>
public class DockSRV : DockFighter
{
    public string SRVType { get; init; }
    public string SRVType_Localised { get; init; }
}