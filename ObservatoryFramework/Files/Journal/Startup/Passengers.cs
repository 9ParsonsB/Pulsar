﻿using System.Text.Json.Serialization;

namespace Observatory.Framework.Files.Journal.Startup;

public class Passengers : JournalBase
{
    [JsonPropertyName("Passengers_Missions_Accepted")]
    public int PassengersMissionsAccepted { get; init; }

    [JsonPropertyName("Passengers_Missions_Bulk")]
    public int PassengersMissionsBulk { get; init; }

    [JsonPropertyName("Passengers_Missions_Delivered")]
    public int PassengersMissionsDelivered { get; init; }

    [JsonPropertyName("Passengers_Missions_Disgruntled")]
    public int PassengersMissionsDisgruntled { get; init; }

    [JsonPropertyName("Passengers_Missions_Ejected")]
    public int PassengersMissionsEjected { get; init; }

    [JsonPropertyName("Passengers_Missions_VIP")]
    public int PassengersMissionsVip { get; init; }
}