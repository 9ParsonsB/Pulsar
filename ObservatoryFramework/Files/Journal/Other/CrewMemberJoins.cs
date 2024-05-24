﻿namespace Observatory.Framework.Files.Journal.Other;

public class CrewMemberJoins : JournalBase
{
    public override string Event => "CrewMemberJoins";
    public string Crew { get; init; }
    public bool Telepresence { get; init; }
}