import type {
	StartStopNotifier,
	Subscriber,
	Unsubscriber,
	Updater,
	Writable,
} from "svelte/store";
import type { HubConnection } from "@microsoft/signalr";
import {
	HubConnectionBuilder,
	HubConnectionState,
	LogLevel,
} from "@microsoft/signalr";

type SignalRPayload = { name: string; data: unknown[] };
type Invalidator<T> = (value?: T) => void;
type SubscribeInvalidateTuple<T> = [Subscriber<T>, Invalidator<T>];
type T = HubConnection;
const noop = () => {};

class ConnectionStore implements Writable<HubConnection> {
	readonly hub: HubConnection;
	readonly subscribers: Array<SubscribeInvalidateTuple<T>> = [];

	public ready = false;
	isLoading: Promise<void> | undefined;

	private start: StartStopNotifier<HubConnection>;
	private stop: Unsubscriber | undefined | null;

	constructor(
		value: HubConnection,
		start: StartStopNotifier<HubConnection> = noop,
	) {
		this.hub = value;
		this.start = start;
		this.hub.onclose(async () => {
			console.log("Lost connection to Event Hub. Attempting to reconnect...");
			await this.hub.start();
		});
	}

	public connect() {
		if (this.hub.state !== HubConnectionState.Disconnected) return;
		this.isLoading = this.hub.start();
		this.isLoading
			.then(() => {
				this.ready = true;
			})
			.catch((e) => {
				console.log(e);
			});
	}

	public set(value: SignalRPayload | HubConnection): Promise<void> {
		if ("name" in value) {
			return this.hub.send(value.name, value.data);
		}
		return Promise.reject();
	}

	public update(updater: Updater<HubConnection>): void {
		updater(this.hub);
	}

	public subscribe(
		run: Subscriber<T>,
		invalidate: Invalidator<T>,
	): Unsubscriber {
		const subscriber: SubscribeInvalidateTuple<T> = [run, invalidate];
		this.subscribers.push(subscriber);

		if (this.subscribers.length === 1) {
			this.stop = this.start ? this.start(this.set, this.update) ?? noop : noop;
		}

		run(this.hub);

		return () => {
			const index = this.subscribers.indexOf(subscriber);
			if (index !== -1) {
				this.subscribers.splice(index, 1);
			}
			if (this.subscribers.length === 0) {
				if (this.stop) this.stop();
				this.stop = null;
			}
		};
	}
}

const conn = new HubConnectionBuilder()
	.withUrl("http://localhost:5000/api/events")
	.configureLogging(LogLevel.Information)
	.withAutomaticReconnect()
	.build();

export const connection = new ConnectionStore(conn);

export default connection;
