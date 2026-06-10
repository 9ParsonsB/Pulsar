namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when a station grants a docking request.
/// </summary>
public class DockingGranted : DockingCancelled
{
    /// <summary>
    ///     Landing pad assigned to the player.
    /// </summary>
    public int LandingPad { get; init; }
}