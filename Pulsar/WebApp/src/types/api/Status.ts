import type Destination from "./Destination";
import type Fuel from "./Fuel";
import type JournalBase from "./JournalBase";

export default interface Status extends JournalBase {
	event: "Status";
	flags: number;
	flags2: number;
	pips: { eng: number; sys: number; wep: number };
	guiFocus: number;
	fuel: Fuel;
	cargo: number;
	legalState: string;
	balance: number;
	destination: Destination;
	FireGroup: number;
}
