﻿using Observatory.Framework.Files.Journal.Travel;

namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class FCMaterlas : FSDJump
{
    public ulong MarketID { get; init; }
    public string CarrierName { get; init; }
    public ulong CarrierID { get; init; }
}