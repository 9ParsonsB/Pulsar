namespace Observatory.Framework.Files.Journal.Other;

public class Music : JournalBase
{
    public override string Event => "Music";
    public string MusicTrack { get; init; }
}