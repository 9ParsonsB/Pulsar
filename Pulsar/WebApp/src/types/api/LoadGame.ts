import type JournalBase from "./JournalBase";

export interface LoadGame extends JournalBase {
	event: "LoadGame";
	commander: string;
	fid: string;
	horizons: boolean;
	odyssey: boolean;
	ship: string;
	ship_Localised: string;
	shipID: number;
	startLanded: boolean;
	startDead: boolean;
	gameMode: string;
	credits: number;
	loan: number;
	shipName: string;
	shipIdent: string;
	fuelLevel: number;
	fuelCapacity: number;
	language: string;
	gameversion: string;
	build: string;
}
export function IsLoadGameEvent(message: JournalBase): message is LoadGame {
	return message.event === "LoadGame";
}
