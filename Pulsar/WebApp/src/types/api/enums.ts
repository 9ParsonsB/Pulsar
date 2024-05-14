export enum StatusFlags {
	Docked = 1,
	Landed = 1 << 1,
	LandingGear = 1 << 2,
	Shields = 1 << 3,
	Supercruise = 1 << 4,
	FAOff = 1 << 5,
	Hardpoints = 1 << 6,
	Wing = 1 << 7,
	Lights = 1 << 8,
	CargoScoop = 1 << 9,
	SilentRunning = 1 << 10,
	FuelScooping = 1 << 11,
	SRVBrake = 1 << 12,
	SRVTurret = 1 << 13,
	SRVProximity = 1 << 14,
	SRVDriveAssist = 1 << 15,
	Masslock = 1 << 16,
	FSDCharging = 1 << 17,
	FSDCooldown = 1 << 18,
	LowFuel = 1 << 19,
	Overheat = 1 << 20,
	LatLongValid = 1 << 21,
	InDanger = 1 << 22,
	Interdiction = 1 << 23,
	MainShip = 1 << 24,
	Fighter = 1 << 25,
	SRV = 1 << 26,
	AnalysisHUD = 1 << 27,
	NightVision = 1 << 28,
	RadialAltitude = 1 << 29,
	FSDJump = 1 << 30,
	SRVHighBeam = 1 << 31,
}

export enum StatusFlags2 {
	OnFoot = 1,
	InTaxi = 1 << 1,
	InMulticrew = 1 << 2,
	OnFootInStation = 1 << 3,
	OnFootOnPlanet = 1 << 4,
	AimDownSight = 1 << 5,
	LowOxygen = 1 << 6,
	LowHealth = 1 << 7,
	Cold = 1 << 8,
	Hot = 1 << 9,
	VeryCold = 1 << 10,
	VeryHot = 1 << 11,
	GlideMode = 1 << 12,
	OnFootInHangar = 1 << 13,
	OnFootInSocialSpace = 1 << 14,
	OnFootExterior = 1 << 15,
	BreathableAtmosphere = 1 << 16,
	TelepresenceMulticrew = 1 << 17,
	PhysicalMulticrew = 1 << 18,
	FsdHyperdriveCharging = 1 << 19,
}

export type LegalStatus =
	| "Clean"
	| "IllegalCargo"
	| "Speeding"
	| "Wanted"
	| "Hostile"
	| "PassengerWanted"
	| "Warrant"
	| "Thargoid";

export enum FocusStatus {
	NoFocus = 0,
	InternalPanel = 1,
	ExternalPanel = 2,
	CommsPanel = 3,
	RolePanel = 4,
	StationServices = 5,
	GalaxyMap = 6,
	SystemMap = 7,
	Orrery = 8,
	FSS = 9,
	SAA = 10,
	Codex = 11,
}
