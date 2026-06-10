namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when the game music 'mood' changes.
/// </summary>
public class Music : JournalBase
{
    public string MusicTrack { get; init; }
}