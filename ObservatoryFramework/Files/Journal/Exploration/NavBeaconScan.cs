namespace Observatory.Framework.Files.Journal.Exploration;

/// <summary>
/// Event generated when scanned a populated system's navigation beacon.
/// </summary>
public class NavBeaconScan : JournalBase
{
    public override string Event => "NavBeaconScan";
    /// <summary>
    /// Number of bodies in system.
    /// </summary>
    public int NumBodies { get; init; }
    /// <summary>
    /// Unique ID of system.
    /// </summary>
    public ulong SystemAddress { get; init; }
}