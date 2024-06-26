﻿namespace Observatory.Framework.Files.Journal.Trade;

public class EjectCargo : JournalBase
{
    public override string Event => "EjectCargo";
    public string Type { get; init; }
    public string Type_Localised { get; init; }
    public int Count { get; init; }
    public int MissionID { get; init; }
    public bool Abandoned { get; init; }
    public string PowerplayOrigin { get; init; }
}