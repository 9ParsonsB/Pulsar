﻿namespace Observatory.Framework.Files.Journal.Odyssey;

public class DeleteSuitLoadout : JournalBase
{
    public override string Event => "DeleteSuitLoadout";
    public ulong SuitID { get; init; }
    public string SuitName { get; init; }
    public string SuitName_Localised { get; init; }
    public ulong LoadoutID { get; init; }
    public string LoadoutName { get; init; }
}