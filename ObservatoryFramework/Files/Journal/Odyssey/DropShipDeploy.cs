﻿namespace Observatory.Framework.Files.Journal.Odyssey;

public class DropShipDeploy : JournalBase
{
    public override string Event => "DropShipDeploy";
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }
    public string Body { get; init; }
    public int BodyID { get; init; }
    public bool OnStation { get; init; }
    public bool OnPlanet { get; init; }
}