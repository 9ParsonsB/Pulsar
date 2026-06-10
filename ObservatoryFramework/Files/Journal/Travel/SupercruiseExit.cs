namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the player exits supercruise at a body.
/// </summary>
public class SupercruiseExit : SupercruiseEntry
{
    /// <summary>
    ///     Name of the body where supercruise ended.
    /// </summary>
    public string Body { get; init; }

    /// <summary>
    ///     Numeric identifier of the body within the system.
    /// </summary>
    public int BodyID { get; init; }

    /// <summary>
    ///     Type of the body where supercruise ended.
    /// </summary>
    public string BodyType { get; init; }
}