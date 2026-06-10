namespace Observatory.Framework.Files.Journal.Combat;

/// <summary>
///     Written when under fire (same time as the Under Attack voice message).
/// </summary>
public class UnderAttack : JournalBase
{
    public string Target { get; init; }
}