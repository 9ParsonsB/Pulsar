namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the player initiates a jump into supercruise or hyperspace.
/// </summary>
public class StartJump : JournalBase
{
    /// <summary>
    ///     Type of jump being initiated, such as supercruise or hyperspace.
    /// </summary>
    public string JumpType { get; init; }

    /// <summary>
    ///     Name of the destination star system for a hyperspace jump.
    /// </summary>
    public string StarSystem { get; init; }

    /// <summary>
    ///     Unique address of the destination star system.
    /// </summary>
    public ulong SystemAddress { get; init; }

    /// <summary>
    ///     Spectral class of the destination star.
    /// </summary>
    public string StarClass { get; init; }

    /// <summary>
    ///     Whether the player is travelling by Apex taxi.
    /// </summary>
    public bool Taxi { get; init; }
}