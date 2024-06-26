﻿namespace Observatory.Framework.Files.Journal.StationServices;

public class RestockVehicle : JournalBase
{
    public override string Event => "RestockVehicle";
    public string Type { get; init; }
    public string Loadout { get; init; }
    public int Cost { get; init; }
    public int Count { get; init; }
}