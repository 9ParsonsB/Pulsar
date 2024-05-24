﻿namespace Observatory.Framework.Files.Journal.StationServices;

public class ShipyardBuy : JournalBase
{
    public override string Event => "ShipyardBuy";
    public ulong MarketID { get; init; }
    public string ShipType { get; init; }
    public string ShipType_Localised { get; init; }
    public long ShipPrice { get; init; }
    public string StoreOldShip { get; init; }
    public ulong StoreShipID { get; init; }
    public string SellOldShip { get; init; }
    public ulong SellShipID { get; init; }
    public long SellPrice { get; init; }
}