import type {StartStopNotifier, Subscriber, Unsubscriber, Updater, Writable,} from "svelte/store";
import type {HubConnection} from "@microsoft/signalr";
import {HubConnectionBuilder, HubConnectionState, LogLevel,} from "@microsoft/signalr";
import {browser} from "$app/environment";

type SignalRPayload = { name: string; data: unknown[] };
type Invalidator<T> = (value?: T) => void;
type SubscribeInvalidateTuple<T> = [Subscriber<T>, Invalidator<T>];
type T = HubConnection;
const noop = () => undefined;

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
            console.log("Lost connection to Event Hub. Attempting to reconnect manually...");
            await this.connect();
        });
    }

    public async connect() {
        if (this.hub.state !== HubConnectionState.Disconnected) return;
        if (this.isLoading) return await this.isLoading;

        this.isLoading = this.hub.start();
        try {
            await this.isLoading;
            this.ready = true;
        } catch (e) {
            console.error("Failed to connect to Event Hub:", e);
        } finally {
            this.isLoading = undefined;
        }
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

const hubUrl = browser
    ? `${window.location.origin}/api/events`
    : "http://127.0.0.1/api/events";

const conn = new HubConnectionBuilder()
    .withUrl(hubUrl)
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

export const connection = new ConnectionStore(conn);

export default connection;
