﻿namespace Observatory.Framework.Files.Journal.StationServices;

public class Shipyard : JournalBase
{
    public override string Event => "Shipyard";
    public ulong MarketID { get; init; }
    /// <summary>
    /// Name of the station at which this event occurred.
    /// </summary>
    public string StationName { get; init; }
    public string StarSystem { get; init; }
}