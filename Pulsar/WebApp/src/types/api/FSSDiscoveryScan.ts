import type JournalBase from "./JournalBase";

export interface FSSDiscoveryScan extends JournalBase {
	event: "FSSDiscoveryScan";
	systemName: string;
	systemAddress: number;
	progress: number;
	bodyCount: number;
	nonBodyCount: number;
	timestamp: string | Date;
}
