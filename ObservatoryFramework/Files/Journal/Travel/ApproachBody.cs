namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the player approaches a body closely enough in supercruise to enter the orbital cruise zone.
/// </summary>
public class ApproachBody : JournalBase
{
    /// <summary>
    ///     Name of the current star system.
    /// </summary>
    public string StarSystem { get; init; }

    /// <summary>
    ///     Unique address of the current star system.
    /// </summary>
    public ulong SystemAddress { get; init; }

    /// <summary>
    ///     Name of the body being approached.
    /// </summary>
    public string Body { get; init; }

    /// <summary>
    ///     Numeric identifier of the body being approached within the system.
    /// </summary>
    public int BodyID { get; init; }
}