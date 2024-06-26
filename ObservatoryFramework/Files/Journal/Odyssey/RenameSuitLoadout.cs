﻿namespace Observatory.Framework.Files.Journal.Odyssey;

public class RenameSuitLoadout : JournalBase
{
    public override string Event => "RenameSuitLoadout";
    public ulong SuitID { get; init; }
    public string SuitName { get; init; }
    public ulong LoadoutID { get; init; }
    public string LoadoutName { get; init; }
}