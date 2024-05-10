<script lang="ts">
    import * as signalR from "@microsoft/signalr"
    import {onMount} from "svelte";
    import axios from "axios";
    import { useQueryClient } from "@sveltestack/svelte-query";
    import { text } from "@sveltejs/kit";
    let x: string | null = $state(null);
    let textarea = $state("");

    interface Welcome {
        flags:       number;
        flags2:      number;
        pips:        number[];
        guiFocus:    number;
        fuel:        Fuel;
        cargo:       number;
        legalState:  string;
        balance:     number;
        destination: Destination;
        timestamp:   Date;
        event:       string;
        FireGroup:   number;
    }

    interface Destination {
        system: number;
        body:   number;
        name:   string;
    }

    interface Fuel {
        fuelMain:      number;
        fuelReservoir: number;
    }

    const queryClient = useQueryClient();

    let status: Welcome | null = $state(null);

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://172.31.0.111:5000/api/events")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    onMount(async () => {
        connection.onclose(async () => {
            console.log(
                "Lost connection to Event Hub. Attempting to reconnect...",
            );
            await connection.start();
        });

        connection.on("StatusUpdated", (message) => {
            status = message as Welcome;
            console.log(status);
        });

        await connection.start();
    });

    const getStatus = async () => {
        var response = await axios.get("http://172.31.0.111:5000/api/status/");
        status = response.data as Welcome;
        textarea = status.event;
    };
</script>

<h1>Status</h1>

<button on:click={getStatus}> GetStatus </button>
<br />

<div>
    {#if status}
        <span>{status.event}</span>
        <span>{status.pips.join(',')}</span>
        <span>{status.destination.name}</span>
        <span>{status.guiFocus}</span>
        <span>{status.cargo}</span>
    {:else}
        <span>No data :(</span>
    {/if}
</div>

<style>
    div {
        display: flex;
        flex-direction: column;
    }
</style>
