namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when another player has joined the wing.
/// </summary>
public class WingAdd : JournalBase
{
    public string Name { get; init; }
}