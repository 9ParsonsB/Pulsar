namespace Observatory.Framework.Files.Journal.Travel;

/// <summary>
///     Written when the player leaves the vicinity of a body.
/// </summary>
public class LeaveBody : JournalBase
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
    ///     Name of the body being left.
    /// </summary>
    public string Body { get; init; }

    /// <summary>
    ///     Numeric identifier of the body being left within the system.
    /// </summary>
    public int BodyID { get; init; }
}