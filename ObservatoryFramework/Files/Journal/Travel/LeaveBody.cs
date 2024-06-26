﻿namespace Observatory.Framework.Files.Journal.Travel;

public class LeaveBody : JournalBase
{
    public override string Event => "LeaveBody";
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }
    public string Body { get; init; }
    public int BodyID { get; init; }
}