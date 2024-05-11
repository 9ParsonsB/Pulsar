import type Destination from "./Destination";
import type Fuel from "./Fuel";

export default interface Status {
	flags: number;
	flags2: number;
	pips: number[];
	guiFocus: number;
	fuel: Fuel;
	cargo: number;
	legalState: string;
	balance: number;
	destination: Destination;
	timestamp: Date;
	event: string;
	FireGroup: number;
}
