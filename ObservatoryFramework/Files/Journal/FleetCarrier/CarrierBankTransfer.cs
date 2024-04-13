﻿namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierBankTransfer : JournalBase
{
    public ulong CarrierID { get; init; }
    public long Deposit { get; init; }
    public long Withdraw { get; init; }
    public long PlayerBalance { get; init; }
    public long CarrierBalance { get; init; }
}