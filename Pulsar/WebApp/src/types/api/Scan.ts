import type JournalBase from "./JournalBase";

export interface Scan extends JournalBase {
	event: "Scan";
	/** may include "Detailed" = via FSS, "AutoScan" = via proximity, "NavBeaconDetail", etc. */
	scanType: string;
	bodyName: string;
	distanceFromArrivalLS: number;
	tidalLock: boolean;
	massEM: number;
	radius: number;
	surfaceGravity: number;
	surfaceTemperature: number;
	surfacePressure: number;
	landable: boolean;
	rotationPeriod: number;
	axialTilt: number;
	/** if the body is a star, the star class */
	starType?: string;
	subclass: number;
	stellarMass: number;
	absoluteMagnitude: number;
	age_MY: number;
	luminosity: string;
	wasDiscovered: boolean;
	wasMapped: boolean;
	starSystem: string;
	systemAddress: number;
	bodyID: number;
	semiMajorAxis: number;
	eccentricity: number;
	orbitalInclination: number;
	periapsis: number;
	orbitalPeriod: number;
	ascendingNode: number;
	meanAnomaly: number;
	timestamp: Date | string;
	parents?: Parent[];
	terraformState?: string;
	/** if the body is a planet, the planet class */
	planetClass?: string;
	atmosphere?: string;
	atmosphereType?: string;
	atmosphereComposition?: AtmosphereComposition[];
	volcanism?: string;
	composition?: Composition;
}

export interface AtmosphereComposition {
	name: string;
	percent: number;
}

export interface Composition {
	ice: number;
	rock: number;
	metal: number;
}

export interface Parent {
	star: number;
}
