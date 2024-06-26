﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DateTimeOffset = System.DateTimeOffset;

namespace Observatory.Framework.Files.Journal;

using System.Text.Json.Serialization;
using Combat;
using Exploration;
using FleetCarrier;
using Odyssey;
using Other;
using Powerplay;
using Squadron;
using Startup;
using StationServices;
using Trade;
using Travel;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "event", UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(Bounty), "Bounty")]
[JsonDerivedType(typeof(CapShipBond), "CapShipBound")]
[JsonDerivedType(typeof(Died), "Died")]
[JsonDerivedType(typeof(EscapeInterdiction), "EscapeInterdiction")]
[JsonDerivedType(typeof(FactionKillBond), "FactionKillBond")]
[JsonDerivedType(typeof(FighterDestroyed), "FighterDestroyed")]
[JsonDerivedType(typeof(HeatDamage), "HeatDamage")]
[JsonDerivedType(typeof(HeatWarning), "HeatWarning")]
[JsonDerivedType(typeof(HullDamage), "HullDamage")]
[JsonDerivedType(typeof(Interdicted), "Interdicted")]
[JsonDerivedType(typeof(Interdiction), "Interdiction")]
[JsonDerivedType(typeof(PVPKill), "PVPKill")]
[JsonDerivedType(typeof(SRVDestroyed), "SRVDestroyed")]
[JsonDerivedType(typeof(ShieldState), "ShieldState")]
[JsonDerivedType(typeof(ShipTargeted), "ShipTargeted")]
[JsonDerivedType(typeof(UnderAttack), "UnderAttack")]
[JsonDerivedType(typeof(BuyExplorationData), "BuyExplorationData")]
[JsonDerivedType(typeof(CodexEntry), "CodexEntry")]
[JsonDerivedType(typeof(DiscoveryScan), "DiscoveryScan")]
[JsonDerivedType(typeof(FSSAllBodiesFound), "FSSAllBodiesFound")]
[JsonDerivedType(typeof(FSSBodySignals), "FSSBodySignals")]
[JsonDerivedType(typeof(FSSDiscoveryScan), "FSSDiscoveryScan")]
[JsonDerivedType(typeof(FSSSignalDiscovered), "FSSSignalDiscovered")]
[JsonDerivedType(typeof(MaterialCollected), "MaterialCollected")]
[JsonDerivedType(typeof(MaterialDiscarded), "MaterialDiscarded")]
[JsonDerivedType(typeof(MaterialDiscovered), "MaterialDiscovered")]
[JsonDerivedType(typeof(MultiSellExplorationData), "MultiSellExplorationData")]
[JsonDerivedType(typeof(NavBeaconScan), "NavBeaconScan")]
[JsonDerivedType(typeof(SAAScanComplete), "SAAScanComplete")]
[JsonDerivedType(typeof(SAASignalsFound), "SAASignalsFound")]
[JsonDerivedType(typeof(Scan), "Scan")]
[JsonDerivedType(typeof(ScanBaryCentre), "ScanBaryCentre")]
[JsonDerivedType(typeof(Screenshot), "Screenshot")]
[JsonDerivedType(typeof(SellExplorationData), "SellExplorationData")]
[JsonDerivedType(typeof(CarrierBankTransfer), "CarrierBankTransfer")]
[JsonDerivedType(typeof(CarrierBuy), "CarrierBuy")]
[JsonDerivedType(typeof(CarrierCancelDecommission), "CarrierCancelDecommission")]
[JsonDerivedType(typeof(CarrierCrewServices), "CarrierCrewServices")]
[JsonDerivedType(typeof(CarrierDecommission), "CarrierDecommission")]
[JsonDerivedType(typeof(CarrierDepositFuel), "CarrierDepositFuel")]
[JsonDerivedType(typeof(CarrierDockingPermission), "CarrierDockingPermission")]
[JsonDerivedType(typeof(CarrierFinance), "CarrierFinance")]
[JsonDerivedType(typeof(CarrierJump), "CarrierJump")]
[JsonDerivedType(typeof(CarrierJumpCancelled), "CarrierJumpCancelled")]
[JsonDerivedType(typeof(CarrierJumpRequest), "CarrierJumpRequest")]
[JsonDerivedType(typeof(CarrierModulePack), "CarrierModulePack")]
[JsonDerivedType(typeof(CarrierShipPack), "CarrierShipPack")]
[JsonDerivedType(typeof(CarrierStats), "CarrierStats")]
[JsonDerivedType(typeof(CarrierTradeOrder), "CarrierTradeOrder")]
[JsonDerivedType(typeof(FleetCarrier.FCMaterials), "FCMaterlas")]
[JsonDerivedType(typeof(BackpackChange), "BackpackChange")]
[JsonDerivedType(typeof(BackpackMaterials), "Backpack")]
[JsonDerivedType(typeof(BookDropship), "BookDropship")]
[JsonDerivedType(typeof(BookTaxi), "BookTaxi")]
[JsonDerivedType(typeof(BuyMicroResources), "BuyMicroResources")]
[JsonDerivedType(typeof(BuySuit), "BuySuit")]
[JsonDerivedType(typeof(BuyWeapon), "BuyWeapon")]
[JsonDerivedType(typeof(CancelDropship), "CancelDropship")]
[JsonDerivedType(typeof(CancelTaxi), "CancelTaxi")]
[JsonDerivedType(typeof(CollectItems), "CollectItems")]
[JsonDerivedType(typeof(CreateSuitLoadout), "CreateSuitLoadout")]
[JsonDerivedType(typeof(DeleteSuitLoadout), "DeleteSuitLoadout")]
[JsonDerivedType(typeof(Disembark), "Disembark")]
[JsonDerivedType(typeof(DropItems), "DropItems")]
[JsonDerivedType(typeof(DropShipDeploy), "DropShipDeploy")]
[JsonDerivedType(typeof(Embark), "Embark")]
[JsonDerivedType(typeof(Odyssey.FCMaterials), "FCMaterials")]
[JsonDerivedType(typeof(LoadoutEquipModule), "LoadoutEquipModule")]
[JsonDerivedType(typeof(LoadoutRemoveModule), "LoadoutRemoveModule")]
[JsonDerivedType(typeof(RenameSuitLoadout), "RenameSuitLoadout")]
[JsonDerivedType(typeof(ScanOrganic), "ScanOrganic")]
[JsonDerivedType(typeof(SellMicroResources), "SellMicroResources")]
[JsonDerivedType(typeof(SellOrganicData), "SellOrganicData")]
[JsonDerivedType(typeof(SellSuit), "SellSuit")]
[JsonDerivedType(typeof(SellWeapon), "SellWeapon")]
[JsonDerivedType(typeof(ShipLockerMaterials), "ShipLocker")]
[JsonDerivedType(typeof(SuitLoadout), "SuitLoadout")]
[JsonDerivedType(typeof(SwitchSuitLoadout) , "SwitchSuitLoadout")]
[JsonDerivedType(typeof(TradeMicroResources), "TradeMicroResources")]
[JsonDerivedType(typeof(TransferMicroResources), "TransferMicroResources")]
[JsonDerivedType(typeof(UpgradeSuit), "UpgradeSuit")]
[JsonDerivedType(typeof(UpgradeWeapon), "UpgradeWeapon")]
[JsonDerivedType(typeof(UseConsumable), "UseConsumable")]
[JsonDerivedType(typeof(AfmuRepairs), "AfmuRepairs")]
[JsonDerivedType(typeof(ApproachSettlement), "ApproachSettlement")]
[JsonDerivedType(typeof(CargoTransfer), "CargoTransfer")]
[JsonDerivedType(typeof(ChangeCrewRole), "ChangeCrewRole")]
[JsonDerivedType(typeof(CockpitBreached), "CockpitBreached")]
[JsonDerivedType(typeof(CommitCrime), "CommitCrime")]
[JsonDerivedType(typeof(Continued), "Continued")]
[JsonDerivedType(typeof(CrewLaunchFighter), "CrewLaunchFighter")]
[JsonDerivedType(typeof(CrewMemberJoins), "CrewMemberJoins")]
[JsonDerivedType(typeof(CrewMemberQuits), "CrewMemberQuits")]
[JsonDerivedType(typeof(CrewMemberRoleChange), "CrewMemberRoleChange")]
[JsonDerivedType(typeof(CrimeVictim), "CrimeVictim")]
[JsonDerivedType(typeof(DataScanned), "DataScanned")]
[JsonDerivedType(typeof(DatalinkScan), "DatalinkScan")]
[JsonDerivedType(typeof(DatalinkVoucher), "DatalinkVoucher")]
[JsonDerivedType(typeof(DockFighter), "DockFighter")]
[JsonDerivedType(typeof(DockSRV), "DockSRV")]
[JsonDerivedType(typeof(EndCrewSession), "EndCrewSession")]
[JsonDerivedType(typeof(FighterRebuilt), "FighterRebuilt")]
[JsonDerivedType(typeof(Friends), "Friends")]
[JsonDerivedType(typeof(FuelScoop), "FuelScoop")]
[JsonDerivedType(typeof(JetConeBoost), "JetConeBoost")]
[JsonDerivedType(typeof(JetConeDamage), "JetConeDamage")]
[JsonDerivedType(typeof(JoinACrew), "JoinACrew")]
[JsonDerivedType(typeof(KickCrewMember), "KickCrewMember")]
[JsonDerivedType(typeof(LaunchDrone), "LaunchDrone")]
[JsonDerivedType(typeof(LaunchFighter), "LaunchFighter")]
[JsonDerivedType(typeof(LaunchSRV), "LaunchSRV")]
[JsonDerivedType(typeof(ModuleInfo), "ModuleInfo")]
[JsonDerivedType(typeof(Music), "Music")]
[JsonDerivedType(typeof(NpcCrewPaidWage), "NpcCrewPaidWage")]
[JsonDerivedType(typeof(NpcCrewRank), "NpcCrewRank")]
[JsonDerivedType(typeof(Promotion), "Promotion")]
[JsonDerivedType(typeof(ProspectedAsteroid), "ProspectedAsteroid")]
[JsonDerivedType(typeof(QuitACrew), "QuitACrew")]
[JsonDerivedType(typeof(RebootRepair), "RebootRepair")]
[JsonDerivedType(typeof(ReceiveText), "ReceiveText")]
[JsonDerivedType(typeof(RepairDrone), "RepairDrone")]
[JsonDerivedType(typeof(ReservoirReplenished), "ReservoirReplenished")]
[JsonDerivedType(typeof(Resurrect), "Resurrect")]
[JsonDerivedType(typeof(Scanned), "Scanned")]
[JsonDerivedType(typeof(SelfDestruct),  "SelfDestruct")]
[JsonDerivedType(typeof(SendText), "SendText")]
[JsonDerivedType(typeof(Shutdown), "Shutdown")]
[JsonDerivedType(typeof(Synthesis), "Synthesis")]
[JsonDerivedType(typeof(SystemsShutdown), "SystemsShutdown")]
[JsonDerivedType(typeof(USSDrop), "USSDrop")]
[JsonDerivedType(typeof(VehicleSwitch), "VehicleSwitch")]
[JsonDerivedType(typeof(WingAdd), "WingAdd")]
[JsonDerivedType(typeof(WingInvite), "WingInvite")]
[JsonDerivedType(typeof(WingJoin), "WingJoin")]
[JsonDerivedType(typeof(WingLeave), "WingLeave")]
[JsonDerivedType(typeof(PowerplayCollect), "PowerplayCollect")]
[JsonDerivedType(typeof(PowerplayDefect), "PowerplayDefect")]
[JsonDerivedType(typeof(PowerplayDeliver), "PowerplayDeliver")]
[JsonDerivedType(typeof(PowerplayFastTrack), "PowerplayFastTrack")]
[JsonDerivedType(typeof(PowerplayJoin), "PowerplayJoin")]
[JsonDerivedType(typeof(PowerplayLeave), "PowerplayLeave")]
[JsonDerivedType(typeof(PowerplaySalary), "PowerplaySalary")]
[JsonDerivedType(typeof(PowerplayVote), "PowerplayVote")]
[JsonDerivedType(typeof(PowerplayVoucher), "PowerplayVoucher")]
[JsonDerivedType(typeof(AppliedToSquadron), "AppliedToSquadron")]
[JsonDerivedType(typeof(DisbandedSquadron),  "DisbandedSquadron")]
[JsonDerivedType(typeof(InvitedToSquadron), "InvitedToSquadron")]
[JsonDerivedType(typeof(JoinedSquadron), "JoinedSquadron")]
[JsonDerivedType(typeof(KickedFromSquadron), "KickedFromSquadron")]
[JsonDerivedType(typeof(LeftSquadron), "LeftSquadron")]
[JsonDerivedType(typeof(SharedBookmarkToSquadron), "SharedBookmarkToSquadron")]
[JsonDerivedType(typeof(SquadronCreated), "SquadronCreated")]
[JsonDerivedType(typeof(SquadronDemotion), "SquadronDemotion")]
[JsonDerivedType(typeof(SquadronPromotion), "SquadronPromotion")]
[JsonDerivedType(typeof(SquadronStartup), "SquadronStartup")]
[JsonDerivedType(typeof(WonATrophyForSquadron), "WonATrophyForSquadron")]
[JsonDerivedType(typeof(Cargo), "Cargo")]
[JsonDerivedType(typeof(ClearSavedGame), "ClearSavedGame")]
[JsonDerivedType(typeof(Commander), "Commander")]
[JsonDerivedType(typeof(FileHeader), "Fileheader")]
[JsonDerivedType(typeof(LoadGame), "LoadGame")]
[JsonDerivedType(typeof(Loadout), "Loadout")]
[JsonDerivedType(typeof(Materials), "Materials")]
[JsonDerivedType(typeof(Missions), "Missions")]
[JsonDerivedType(typeof(NewCommander), "NewCommander")]
[JsonDerivedType(typeof(Passengers), "Passengers")]
[JsonDerivedType(typeof(Startup.Powerplay), "Powerplay")]
[JsonDerivedType(typeof(Progress), "Progress")]
[JsonDerivedType(typeof(Rank), "Rank")]
[JsonDerivedType(typeof(Reputation), "Reputation")]
[JsonDerivedType(typeof(Statistics), "Statistics")]
[JsonDerivedType(typeof(BuyAmmo), "BuyAmmo")]
[JsonDerivedType(typeof(BuyDrones), "BuyDrones")]
[JsonDerivedType(typeof(CargoDepot), "CargoDepot")]
[JsonDerivedType(typeof(ClearImpound), "ClearImpound")]
[JsonDerivedType(typeof(CommunityGoal), "CommunityGoal")]
[JsonDerivedType(typeof(CommunityGoalDiscard), "CommunityGoalDiscard")]
[JsonDerivedType(typeof(CommunityGoalJoin), "CommunityGoalJoin")]
[JsonDerivedType(typeof(CommunityGoalReward), "CommunityGoalReward")]
[JsonDerivedType(typeof(CrewAssign), "CrewAssign")]
[JsonDerivedType(typeof(CrewFire), "CrewFire")]
[JsonDerivedType(typeof(CrewHire), "CrewHire")]
[JsonDerivedType(typeof(EngineerApply), "EngineerApply")]
[JsonDerivedType(typeof(EngineerContribution), "EngineerContribution")]
[JsonDerivedType(typeof(EngineerCraft), "EngineerCraft")]
[JsonDerivedType(typeof(EngineerLegacyConvert), "EngineerLegacyConvert")]
[JsonDerivedType(typeof(EngineerProgress), "EngineerProgress")]
[JsonDerivedType(typeof(FetchRemoteModule), "FetchRemoteModule")]
[JsonDerivedType(typeof(Market), "Market")]
[JsonDerivedType(typeof(MassModuleStore), "MassModuleStore")]
[JsonDerivedType(typeof(MaterialTrade), "MaterialTrade")]
[JsonDerivedType(typeof(MissionAbandoned), "MissionAbandoned")]
[JsonDerivedType(typeof(MissionAccepted), "MissionAccepted")]
[JsonDerivedType(typeof(MissionCompleted), "MissionCompleted")]
[JsonDerivedType(typeof(MissionFailed), "MissionFailed")]
[JsonDerivedType(typeof(MissionRedirected), "MissionRedirected")]
[JsonDerivedType(typeof(ModuleBuy), "ModuleBuy")]
[JsonDerivedType(typeof(ModuleRetrieve), "ModuleRetrieve")]
[JsonDerivedType(typeof(ModuleSell), "ModuleSell")]
[JsonDerivedType(typeof(ModuleSellRemote), "ModuleSellRemote")]
[JsonDerivedType(typeof(ModuleStore), "ModuleStore")]
[JsonDerivedType(typeof(ModuleSwap), "ModuleSwap")]
[JsonDerivedType(typeof(ModuleBuyAndStore), "ModuleBuyAndStore")]
[JsonDerivedType(typeof(Outfitting), "Outfitting")]
[JsonDerivedType(typeof(PayBounties), "PayBounties")]
[JsonDerivedType(typeof(PayFines), "PayFines")]
[JsonDerivedType(typeof(PayLegacyFines), "PayLegacyFines")]
[JsonDerivedType(typeof(RedeemVoucher), "RedeemVoucher")]
[JsonDerivedType(typeof(RefuelAll), "RefuelAll")]
[JsonDerivedType(typeof(RefuelPartial), "RefuelPartial")]
[JsonDerivedType(typeof(Repair), "Repair")]
[JsonDerivedType(typeof(RepairAll), "RepairAll")]
[JsonDerivedType(typeof(RestockVehicle), "RestockVehicle")]
[JsonDerivedType(typeof(ScientificResearch), "ScientificResearch")]
[JsonDerivedType(typeof(SearchAndRescue), "SearchAndRescue")]
[JsonDerivedType(typeof(SellDrones), "SellDrones")]
[JsonDerivedType(typeof(SellShipOnRebuy), "SellShipOnRebuy")]
[JsonDerivedType(typeof(SetUserShipName), "SetUserShipName")]
[JsonDerivedType(typeof(Shipyard), "Shipyard")]
[JsonDerivedType(typeof(ShipyardBuy), "ShipyardBuy")]
[JsonDerivedType(typeof(ShipyardNew), "ShipyardNew")]
[JsonDerivedType(typeof(ShipyardSell), "ShipyardSell")]
[JsonDerivedType(typeof(ShipyardSwap), "ShipyardSwap")]
[JsonDerivedType(typeof(ShipyardTransfer), "ShipyardTransfer")]
[JsonDerivedType(typeof(StoredModules), "StoredModules")]
[JsonDerivedType(typeof(StoredShips), "StoredShips")]
[JsonDerivedType(typeof(TechnologyBroker), "TechnologyBroker")]
[JsonDerivedType(typeof(AsteroidCracked), "AsteroidCracked")]
[JsonDerivedType(typeof(BuyTradeData), "BuyTradeData")]
[JsonDerivedType(typeof(CollectCargo), "CollectCargo")]
[JsonDerivedType(typeof(EjectCargo), "EjectCargo")]
[JsonDerivedType(typeof(MarketBuy), "MarketBuy")]
[JsonDerivedType(typeof(MarketSell), "MarketSell")]
[JsonDerivedType(typeof(MiningRefined), "MiningRefined")]
[JsonDerivedType(typeof(ApproachBody), "ApproachBody")]
[JsonDerivedType(typeof(Docked), "Docked")]
[JsonDerivedType(typeof(DockingCancelled), "DockingCancelled")]
[JsonDerivedType(typeof(DockingDenied), "DockingDenied")]
[JsonDerivedType(typeof(DockingGranted), "DockingGranted")]
[JsonDerivedType(typeof(DockingRequested), "DockingRequested")]
[JsonDerivedType(typeof(DockingTimeout), "DockingTimeout")]
[JsonDerivedType(typeof(FSDJump), "FSDJump")]
[JsonDerivedType(typeof(FSDTarget), "FSDTarget")]
[JsonDerivedType(typeof(LeaveBody), "LeaveBody")]
[JsonDerivedType(typeof(Liftoff), "Liftoff")]
[JsonDerivedType(typeof(Location), "Location")]
[JsonDerivedType(typeof(NavRoute), "NavRoute")]
[JsonDerivedType(typeof(NavRouteClear), "NavRouteClear")]
[JsonDerivedType(typeof(StartJump), "StartJump")]
[JsonDerivedType(typeof(SupercruiseDestinationDrop), "SupercruiseDestinationDrop")]
[JsonDerivedType(typeof(SupercruiseEntry), "SupercruiseEntry")]
[JsonDerivedType(typeof(SupercruiseExit), "SupercruiseExit")]
[JsonDerivedType(typeof(Touchdown), "Touchdown")]
[JsonDerivedType(typeof(Undocked), "Undocked")]
[JsonDerivedType(typeof(Status), "Status")]
public abstract class JournalBase
{
    [JsonPropertyName("timestamp")]
    [Key]
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// As this is used for the JsonPolymorphic attribute, this will not be deserilized.
    /// </summary>
    [JsonPropertyName("event")]
    public abstract string Event { get; }
    
    [JsonExtensionData]
    [IgnoreDataMember]
    [NotMapped]
    public Dictionary<string, object> AdditionalProperties { get; init; }
}
        