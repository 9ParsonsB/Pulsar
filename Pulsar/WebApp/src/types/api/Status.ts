import type Destination from "./Destination";
import type Fuel from "./Fuel";
import type JournalBase from "./JournalBase";
import type {
	FocusStatus,
	LegalStatus,
	StatusFlags,
	StatusFlags2,
} from "./enums";

export default interface Status extends JournalBase {
	event: "Status";
	flags: StatusFlags;
	flags2: StatusFlags2;
	pips: { eng: number; sys: number; wep: number };
	guiFocus: FocusStatus;
	fuel: Fuel;
	cargo: number;
	legalState: LegalStatus;
	balance: number;
	destination: Destination;
	FireGroup: number;
}
