﻿using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class CrimeVictim : JournalBase
{
    public override string Event => "CrimeVictim";
    public string Offender { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CrimeType CrimeType { get; init; }
    public int Fine { get; init; }
    public int Bounty { get; init; }
}