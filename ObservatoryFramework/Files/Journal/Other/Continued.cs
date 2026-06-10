namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written if the journal file grows to 500k lines, we write this event, close the file, and start a new one.
/// </summary>
public class Continued : JournalBase
{
    public int Part { get; init; }
}