import type JournalBase from "./JournalBase";

export interface Signal {
	type: string;
	type_Localised: string;
	count: number;
}

export interface GenusType {
	Genus: string;
	Genus_Localised: string;
}

interface BodySignalBase extends JournalBase {
	SystemAddress: bigint;
	BodyName: string;
	BodyID: bigint;
	Signals: Signal[];
	Genuses: GenusType[];
}

export interface SAASignalsFound extends BodySignalBase {
	event: "SAASignalsFound";
}

export interface FSSBodySignals extends BodySignalBase {
	event: "FSSBodySignals";
}
