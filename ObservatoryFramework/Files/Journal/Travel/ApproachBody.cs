﻿namespace Observatory.Framework.Files.Journal.Travel;

public class ApproachBody : JournalBase
{
    public override string Event => "ApproachBody";
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }
    public string Body { get; init; }
    public int BodyID { get; init; }
}